using Lottie.Forms;
using MLToolkit.Forms.SwipeCardView;
using MLToolkit.Forms.SwipeCardView.Core;
using PrimeOrNot.Model;
using PrimeOrNot.ViewModel;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PrimeOrNot.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TinderPage : ContentPage
    {

        public SwipeCardDirection SupportedDraggingDirections => SwipeCardDirection.Right
                                                         | SwipeCardDirection.Left
                                                         | SwipeCardDirection.Up
                                                         | SwipeCardDirection.Down;
        public SwipeCardDirection SupportedSwipingDirections => SwipeCardDirection.Right
                                                         | SwipeCardDirection.Left
                                                         | SwipeCardDirection.Up
                                                         | SwipeCardDirection.Down;
     

        private static Stopwatch stopwatch;
        private static double sec15 = 15000;
        private static bool finished = false;
        double timer = sec15;
        bool nopeVisible = true;
        Number LastNumber = null;
        public TinderPage()
        {
            InitializeComponent();
            BindingContext = new TinderPageViewModel();

            SwipeCardView.Dragging += OnDragging;
            SwipeCardView.Swiped += OnSwiping;
            stopwatch = new Stopwatch();

            stopwatch.Start();
            finished = false;
            nopeVisible = true;
           
        }

        private void OnDislikeClicked(object sender, EventArgs e)
        {
            SwipeCardView.InvokeSwipe(SwipeCardDirection.Left);
        }

        private void OnSuperLikeClicked(object sender, EventArgs e)
        {
            SwipeCardView.InvokeSwipe(SwipeCardDirection.Up);
        }

        private void OnLikeClicked(object sender, EventArgs e)
        {

            SwipeCardView.InvokeSwipe(SwipeCardDirection.Right);




        }
        private async void OnSwiping(object sender, SwipedCardEventArgs e)
        {
            if (finished)
            {
                if (!nopeVisible)
                {
                    if (((Number)e.Item).Id == LastNumber.Id)
                    {
                        bool answer = await DisplayAlert("Next game?", "Play another game?", "Yes", "No");
                        if (answer)
                          await  finish_fun(0);
                        else
                            await finish_fun(1);
                    }
                }
                return;
            }
            uint timeout = 50;

            if ((e.Direction == SwipeCardDirection.Left && !((Number)e.Item).Prime)
                ||
                (e.Direction == SwipeCardDirection.Right && ((Number)e.Item).Prime))
            {
                ((Number)e.Item).Correct = true;


                ViewModel.ViewExtensions.CancelAnimation(MyStack);
                MyStack.ColorTo(Color.Green, Color.White, c => MyStack.BackgroundColor = c, 500);
                var v = new AnimationView();
                v.Animation = "4964-check-mark-success-animation.json";
                v.IsPlaying = true;
                v.WidthRequest = 25;
                v.HeightRequest = 25;


                FlexL.Children.Add(v);

                timer = Math.Min(sec15, timer - stopwatch.Elapsed.TotalMilliseconds + 1000);
                var segment = (float)Math.Min(1, 1 - (timer) / sec15);
                MyAnimationView.PlayProgressSegment((float)segment, 1);
                stopwatch.Restart();


            }
            else if ((e.Direction == SwipeCardDirection.Left && ((Number)e.Item).Prime)
                ||
                (e.Direction == SwipeCardDirection.Right && !((Number)e.Item).Prime))
            {
                ((Number)e.Item).Correct = false;
                ViewModel.ViewExtensions.CancelAnimation(MyStack);
                MyStack.ColorTo(Color.Red, Color.White, c => MyStack.BackgroundColor = c, 500);
                await MyStack.TranslateTo(-15, 0, timeout);
                await MyStack.TranslateTo(15, 0, timeout);
                await MyStack.TranslateTo(-9, 0, timeout);
                await MyStack.TranslateTo(9, 0, timeout);
                await MyStack.TranslateTo(-5, 0, timeout);
                await MyStack.TranslateTo(5, 0, timeout);
                await MyStack.TranslateTo(-2, 0, timeout);
                await MyStack.TranslateTo(2, 0, timeout);
                MyStack.TranslationX = 0;

                var v = new AnimationView();
                v.Animation = "4970-unapproved-cross.json";
                v.IsPlaying = true;
                v.WidthRequest = 25;
                v.HeightRequest = 25;

                timer = Math.Max(0, timer - stopwatch.Elapsed.TotalMilliseconds - 2000);
                var segment = (float)Math.Min(1, 1 - (timer) / sec15);
                MyAnimationView.PlayProgressSegment((float)segment, 1);
                stopwatch.Restart();


                FlexL.Children.Add(v);
            }
            else if (e.Direction == SwipeCardDirection.Up)
            {
                ViewModel.ViewExtensions.CancelAnimation(MyStack);
                MyStack.ColorTo(Color.Yellow, Color.White, c => MyStack.BackgroundColor = c, 500);

                var v = new AnimationView();
                v.Animation = "4975-question-mark.json";
                v.IsPlaying = true;
                v.WidthRequest = 25;
                v.HeightRequest = 25;


                FlexL.Children.Add(v);


            }



        }

        private void OnDragging(object sender, DraggingCardEventArgs e)
        {
            var view = (Xamarin.Forms.View)sender;
            var nopeFrame = view.FindByName<Frame>("NopeFrame");
            var likeFrame = view.FindByName<Frame>("LikeFrame");
            var superLikeFrame = view.FindByName<Frame>("SuperLikeFrame");
            var threshold = (BindingContext as TinderPageViewModel).Threshold;
            nopeFrame.IsVisible = nopeVisible;
            var draggedXPercent = e.DistanceDraggedX / threshold;

            var draggedYPercent = e.DistanceDraggedY / threshold;

            switch (e.Position)
            {
                case DraggingCardPosition.Start:
                    nopeFrame.Opacity = 0;
                    likeFrame.Opacity = 0;
                    superLikeFrame.Opacity = 0;
                    //nopeButton.Scale = 1;
                    //likeButton.Scale = 1;
                    //superLikeButton.Scale = 1;
                    break;

                case DraggingCardPosition.UnderThreshold:
                    if (e.Direction == SwipeCardDirection.Left)
                    {
                        nopeFrame.Opacity = (-1) * draggedXPercent;
                        //nopeButton.Scale = 1 + draggedXPercent / 2;
                        superLikeFrame.Opacity = 0;
                        //superLikeButton.Scale = 1;
                    }
                    else if (e.Direction == SwipeCardDirection.Right)
                    {
                        likeFrame.Opacity = draggedXPercent;
                        //likeButton.Scale = 1 - draggedXPercent / 2;
                        superLikeFrame.Opacity = 0;
                        //superLikeButton.Scale = 1;
                    }
                    else if (e.Direction == SwipeCardDirection.Up)
                    {
                        nopeFrame.Opacity = 0;
                        likeFrame.Opacity = 0;
                        //nopeButton.Scale = 1;
                        //likeButton.Scale = 1;
                        superLikeFrame.Opacity = (-1) * draggedYPercent;
                        //superLikeButton.Scale = 1 + draggedYPercent / 2;
                    }
                    break;

                case DraggingCardPosition.OverThreshold:
                    if (e.Direction == SwipeCardDirection.Left)
                    {
                        nopeFrame.Opacity = 1;
                        superLikeFrame.Opacity = 0;
                    }
                    else if (e.Direction == SwipeCardDirection.Right)
                    {
                        likeFrame.Opacity = 1;
                        superLikeFrame.Opacity = 0;
                    }
                    else if (e.Direction == SwipeCardDirection.Up)
                    {
                        nopeFrame.Opacity = 0;
                        likeFrame.Opacity = 0;
                        superLikeFrame.Opacity = 1;
                    }
                    break;

                case DraggingCardPosition.FinishedUnderThreshold:
                    nopeFrame.Opacity = 0;
                    likeFrame.Opacity = 0;
                    superLikeFrame.Opacity = 0;
                    //nopeButton.Scale = 1;
                    //likeButton.Scale = 1;
                    //superLikeButton.Scale = 1;
                    break;

                case DraggingCardPosition.FinishedOverThreshold:
                    nopeFrame.Opacity = 0;
                    likeFrame.Opacity = 0;
                    superLikeFrame.Opacity = 0;
                    //nopeButton.Scale = 1;
                    //likeButton.Scale = 1;
                    //superLikeButton.Scale = 1;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async void AnimationView_OnFinish(object sender2, EventArgs e)
        {
            if (finished)
                return;

            finished = true;

            //Thread.Sleep(2000);
            //bool answer = await DisplayAlert("Time is up!", "Would you like to play another game?", "Yes", "No");


            var a = true;
            var result = 1;
            while (a)
            {
                try
                {
                    var inputView = new Popup3("TIME IS UP!");

                    // create the Transparent Popup Page
                    // of type string since we need a string return
                    var popup = new InputAlertDialogBase<string>(inputView);
                    inputView.CloseButtonEventHandler +=
                     (sender, obj) =>
                     {
                         popup.PageClosedTaskCompletionSource.SetResult("1");
                     };
                    inputView.PlayButtonEventHandler +=
               (sender, obj) =>
               {
                   popup.PageClosedTaskCompletionSource.SetResult("0");
               };
                    inputView.ResultsButtonEventHandler +=
               (sender, obj) =>
               {
                   popup.PageClosedTaskCompletionSource.SetResult("2");
               };

                    // Push the page to Navigation Stack
                    await Navigation.PushPopupAsync(popup);

                    // await for the user to enter the text input
                    result = Convert.ToInt32(await popup.PageClosedTask);

                    // Pop the page from Navigation Stack
                    await Navigation.PopPopupAsync();
                    a = false;
                }
                catch (Exception ex)
                {

                }
            }
            a = true;
            while (a)
            {
                try
                {
                    await finish_fun(result);
                    a = false;
                }
                catch (Exception ex)
                {

                }
            }

        }
     
        private async Task finish_fun(int answer)
        {
            if (answer == 2) // results
            {
                List<Number> results = new List<Number>();
                for (int i = 0; i < FlexL.Children.Count; i++)
                {
                    results.Add((Number)SwipeCardView.ItemsSource[i]);
                    LastNumber = (Number)SwipeCardView.ItemsSource[i];
                }
                BindingContext = new TinderPageViewModel(results);
                SwipeCardView.SupportedDraggingDirections = SwipeCardDirection.Left;
                SwipeCardView.SupportedSwipeDirections = SwipeCardDirection.Left;

                nopeVisible = false;

            }
            else if (answer == 0) // play again
            {
                BindingContext = new TinderPageViewModel();
                MyAnimationView.PlayProgressSegment(0, 1);
                timer = sec15;
                stopwatch.Restart();
                FlexL.Children.Clear();
                finished = false;
                nopeVisible = true;
                SwipeCardView.SupportedDraggingDirections = SupportedDraggingDirections;
                SwipeCardView.SupportedSwipeDirections = SupportedSwipingDirections;

            }
            else if (answer == 1) // quit
            {
                await Navigation.PopAsync();
                finished = false;
            }
        }


    }
}