using System;
using System.Collections.Generic;
using CarAssessment.Views;
using CarAssessment.Views.AbstractViews;
using Xamarin.Forms;

// Class allows switch on and off groups and colouring the headers
// according to the validation state
namespace CarAssessment.Layout
{
    public class LayoutController
    {
        private readonly List<View> allViews;
        private NewItemPageBase contentPage;
        private int displayedGroup;

        public LayoutController(NewItemPageBase contentPage, CreationMode creationMode)
        {
            allViews = new List<View>();
            this.contentPage = contentPage;
            init(creationMode);
        }

        private void init(CreationMode creationMode)
        {
            iterate(contentPage.Content);
            DisplayedGroup = creationMode == CreationMode.Direct ? 1 : 0;
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

        internal int DisplayedGroup
        {
            get => displayedGroup;
            set {
                displayedGroup = value;
                allViews.ForEach((view) => view.IsVisible = int.Parse(view.AutomationId) == displayedGroup || displayedGroup > 0 && view.AutomationId=="99");
				contentPage.HandleSpecialFields(displayedGroup);
                contentPage.PrevArrowButtonVisiblity = displayedGroup > 1;
                contentPage.NextArrowButtonVisiblity = displayedGroup < 12;
                if (value == 11) {
                    contentPage.EnterDeclarationOfAssignment();
				} else if (value == 12) {
                    contentPage.EnterAdvocateAssignment();
				}
            }
        }

        private void Validate(int Group) {

		}

        public void Next() {
            Validate(DisplayedGroup);
            contentPage.PrevArrowButtonVisiblity = true;
            if (DisplayedGroup < 12) {
                DisplayedGroup++;
                if (DisplayedGroup == 12) {
                    contentPage.NextArrowButtonVisiblity = false;
                }
            }
		}

        public void Previous() {
            Validate(DisplayedGroup);
            contentPage.NextArrowButtonVisiblity = true;
            if (DisplayedGroup > 1) {
                DisplayedGroup--;
                if (DisplayedGroup == 1) {
                    contentPage.PrevArrowButtonVisiblity = false;
				}
            }
		}

        public void Up() {
            Validate(DisplayedGroup);
            DisplayedGroup = 0;
		}
    }
}
