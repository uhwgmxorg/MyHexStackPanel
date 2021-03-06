﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyHexStackPanel
{
    /// <summary>
    /// Interaktionslogik für HexMessageContainerUC.xaml
    /// </summary>
    public partial class HexMessageContainerUC : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Dependencie Propertys
        public HexMessageContainerUCAction MyUserControlActionObj
        {
            get
            {
                return (HexMessageContainerUCAction)GetValue(MyUserControlActionProperty);
            }
            set { SetValue(MyUserControlActionProperty, value); }
        }
        public static readonly DependencyProperty MyUserControlActionProperty =
            DependencyProperty.Register("MyUserControlActionObj", typeof(HexMessageContainerUCAction), typeof(HexMessageContainerUC), new PropertyMetadata(null));
        #endregion

        /// <summary>
        /// Constructur
        /// </summary>
        public HexMessageContainerUC()
        {
            InitializeComponent();
            DataContext = this;

            MyUserControlActionObj = HexMessageContainerUCAction.Instance;

            MyUserControlActionObj.AddMessageEvent += AddMessageHandler;
        }

        /******************************/
        /*       Button Events        */
        /******************************/
        #region Button Events

        #endregion
        /******************************/
        /*      Menu Events          */
        /******************************/
        #region Menu Events

        #endregion
        /******************************/
        /*      Other Events          */
        /******************************/
        #region Other Events

        /// <summary>
        /// AddMessageHandler
        /// </summary>
        /// <param name="message"></param>
        /// <param name="direction"></param>
        private void AddMessageHandler(byte[] message, Direction direction)
        {
            AddMessage(message,direction);
        }

        #endregion
        /******************************/
        /*      Other Functions       */
        /******************************/
        #region Other Functions

        /// <summary>
        /// AddMessage
        /// </summary>
        /// <param name="message"></param>
        /// <param name="direction"></param>
        public void AddMessage(byte[] message, Direction direction)
        {
            LineStackPanel.Children.Add(new HexMessageUC { HexContentByte = message, MessageDirection = direction });
            ScrollViewer1.ScrollToBottom();
        }

        /// <summary>
        /// OnPropertyChanged
        /// </summary>
        /// <param name="p"></param>
        private void OnPropertyChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

        #endregion
    }

    /// <summary>
    /// class HexMessageContainerUCAction
    /// for take action on the UC i.e. 
    /// add a message
    /// </summary>
    public class HexMessageContainerUCAction
    {

        private static HexMessageContainerUCAction instance;

        public static HexMessageContainerUCAction Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HexMessageContainerUCAction();
                }
                return instance;
            }
        }

        public delegate void SignalEventHandler(byte[] message,Direction direction);
        public event SignalEventHandler AddMessageEvent;

        /// <summary>
        /// Constructor
        /// </summary>
        private HexMessageContainerUCAction()
        {
        }

        /// <summary>
        /// AddMessage
        /// </summary>
        /// <param name="message"></param>
        /// <param name="direction"></param>
        public void AddMessage(byte[] message,Direction direction)
        {
            if (AddMessageEvent != null) AddMessageEvent(message,direction);
        }
    }

    /// <summary>
    /// class AutoScrollBehavior
    /// for change window size
    /// </summary>
    public static class AutoScrollBehavior
    {
        public static readonly DependencyProperty AutoScrollProperty =
            DependencyProperty.RegisterAttached("AutoScroll", typeof(bool), typeof(AutoScrollBehavior), new PropertyMetadata(false, AutoScrollPropertyChanged));


        public static void AutoScrollPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var scrollViewer = obj as ScrollViewer;
            if (scrollViewer != null && (bool)args.NewValue)
            {
                scrollViewer.SizeChanged += ScrollViewer_SizeChanged;
                scrollViewer.ScrollToEnd();
            }
            else
            {
                scrollViewer.LayoutUpdated -= ScrollViewer_SizeChanged;
            }
        }

        private static void ScrollViewer_SizeChanged(object sender, EventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            if(scrollViewer != null)
                scrollViewer.ScrollToEnd();
        }

        public static bool GetAutoScroll(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoScrollProperty);
        }

        public static void SetAutoScroll(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoScrollProperty, value);
        }
    }
}
