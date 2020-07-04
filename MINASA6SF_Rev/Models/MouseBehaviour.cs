using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace MINASA6SF_Rev.Models
{
    public class MouseBehaviour : Behavior<Button>
    {
        protected override void OnAttached()
        {
            
            this.AssociatedObject.MouseDown += AssociatedObject_MouseDown;
        }

        public void AssociatedObject_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("MouseDown called");
        }

        protected override void OnDetaching()
        {
            Debug.WriteLine("OnDetaching called");
            AssociatedObject.MouseDown -= AssociatedObject_MouseDown;
        }
    }
}
