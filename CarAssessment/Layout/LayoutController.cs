using System;
using System.Collections.Generic;
using Xamarin.Forms;

// Class allows switch on and off groups and colouring the headers
// according to the validation state
namespace CarAssessment.Layout
{
    public class LayoutController
    {
        private readonly List<View> allViews;
        private ContentPage contentPage;
        private int displayedGroup;

        public LayoutController(ContentPage contentPage)
        {
            allViews = new List<View>();
            this.contentPage = contentPage;
            init();
        }

        private void init()
        {
            iterate(contentPage.Content);
            DisplayedGroup = 2;
        }

        private void display(View view) {
            Console.WriteLine(view.ToString());
		}

        private void iterate(View view)
        {
            display(view);
            if (view.AutomationId != null)
            {
                allViews.Add(view);
            }
            var type = view.GetType();
            if (type == typeof(StackLayout))
            {
                iterateChildren(view as StackLayout);
            }

            if (type == typeof(Grid))
            {
                iterateChildren(view as Grid);
            }

            if (type == typeof(ScrollView)) {
                iterate((view as ScrollView).Content);
			}
        }

        private void iterateChildren(StackLayout stackLayout)
        {
            foreach(var child in stackLayout.Children)
            {
                iterate(child);
            }
        }

        private void iterateChildren(Grid grid)
        {
            foreach(var child in grid.Children)
            {
                iterate(child);
            }
        }

        private int DisplayedGroup
        {
            get => displayedGroup;
            set {
                displayedGroup = value;
                allViews.ForEach((view) => view.IsVisible = int.Parse(view.AutomationId) == displayedGroup);
            }
        }

        public void Next() {
            DisplayedGroup++;
		}

        public void Previous() {
            DisplayedGroup--;
		}

        public void Up() {

		}
    }
}
