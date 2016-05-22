﻿using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace ScreenToGif.Controls
{
    /// <summary>
    /// Numeric only control with up and down buttons to change the value.
    /// </summary>
    public class NumericUpDown : Control
    {
        #region Variables

        private RepeatButton _upButton;
        private RepeatButton _downButton;
        private TextBox _textBox;

        public static readonly DependencyProperty MaximumProperty;
        public static readonly DependencyProperty MinimumProperty;
        public static readonly DependencyProperty ValueProperty;
        public static readonly DependencyProperty StepProperty;

        #endregion

        #region Properties

        /// <summary>
        /// The maximum value of the numeric up and down.
        /// </summary>
        [Description("The maximum value of the numeric up and down.")]
        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set
            {
                //Change de actual value to the range.
                if (value < Value)
                {
                    Value = value;
                }

                SetValue(MaximumProperty, value);
            }
        }

        /// <summary>
        /// The minimu value of the numeric up and down.
        /// </summary>
        [Description("The minimum value of the numeric up and down.")]
        public int Minimum
        {
            get { return (int)GetValue(MinimumProperty); }
            set
            {
                //Change de actual value to the range.
                if (value > Value)
                {
                    Value = value;
                }

                SetValue(MinimumProperty, value);
            }
        }

        /// <summary>
        /// The actual value of the numeric up and down.
        /// </summary>
        [Description("The actual value of the numeric up and down.")]
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set
            {
                SetCurrentValue(ValueProperty, value);

                if (_textBox != null)
                    _textBox.Text = Value.ToString();

                if (ValueChanged != null)
                    ValueChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// The Increment/Decrement value.
        /// </summary>
        [Description("The Increment/Decrement value.")]
        public int StepValue
        {
            get { return (int)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        #endregion

        #region Events

        public event EventHandler ValueChanged;
        public static event EventHandler InternalValueChanged;

        #endregion

        static NumericUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(typeof(NumericUpDown)));

            MaximumProperty = DependencyProperty.Register("Maximum", typeof(int), typeof(NumericUpDown), new UIPropertyMetadata(40));
            MinimumProperty = DependencyProperty.Register("Minimum", typeof(int), typeof(NumericUpDown), new UIPropertyMetadata(1));
            StepProperty = DependencyProperty.Register("StepValue", typeof(int), typeof(NumericUpDown), new FrameworkPropertyMetadata(1));
            ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(NumericUpDown), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.None, PropertyChangedCallback));
        }

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (InternalValueChanged != null)
                InternalValueChanged(null, null);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _upButton = Template.FindName("UpButton", this) as RepeatButton;
            _downButton = Template.FindName("DownButton", this) as RepeatButton;
            _textBox = Template.FindName("InternalBox", this) as TextBox;

            _textBox.Text = Minimum.ToString();
            _upButton.Click += _UpButton_Click;
            _downButton.Click += _DownButton_Click;

            _textBox.TextChanged += _textBox_TextChanged;
            _textBox.PreviewTextInput += _textBox_PreviewTextInput;
            _textBox.MouseWheel += _textBox_MouseWheel;
            _textBox.LostFocus += _textBox_LostFocus;
            _textBox.KeyDown += TextBox_KeyDown;

            Value = Value == 1 ? Minimum : Value;

            AddHandler(DataObject.PastingEvent, new DataObjectPastingEventHandler(PastingEvent));
            InternalValueChanged += NumericTextBox_ValueChanged;
        }

        private static bool IsTextDisallowed(string text)
        {
            var regex = new Regex("[^-?0-9]+");
            return regex.IsMatch(text);
        }

        #region Event Handlers

        private void _DownButton_Click(object sender, RoutedEventArgs e)
        {
            if (Value > Minimum)
            {
                Value -= StepValue;
                if (Value < Minimum)
                    Value = Minimum;

                //_textBox.Text = Value.ToString();
            }
        }

        private void _UpButton_Click(object sender, RoutedEventArgs e)
        {
            if (Value < Maximum)
            {
                Value += StepValue;
                if (Value > Maximum)
                    Value = Maximum;

                //_textBox.Text = Value.ToString();
            }
        }


        private void NumericTextBox_ValueChanged(object sender, EventArgs e)
        {
            InternalValueChanged -= NumericTextBox_ValueChanged;

            if (Value > Maximum)
                Value = Maximum;

            else if (Value < Minimum)
                Value = Minimum;

            InternalValueChanged += NumericTextBox_ValueChanged;

            _textBox.Text = Value.ToString();
        }

        private void _textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (String.IsNullOrEmpty(e.Text))
            {
                e.Handled = true;
                return;
            }

            if (IsTextDisallowed(e.Text))
            {
                e.Handled = true;
            }
        }

        private void _textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;

            if (textBox == null) return;

            Value = Convert.ToInt32(textBox.Text);
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;

            if (textBox == null) return;

            if (e.Key != Key.Enter) return;

            Value = Convert.ToInt32(textBox.Text);
        }

        private void _textBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var textBox = sender as TextBox;

            if (textBox == null) return;

            int step = Keyboard.Modifiers == (ModifierKeys.Shift | ModifierKeys.Control) ? 50 :
                Keyboard.Modifiers == ModifierKeys.Shift ? 10
                : Keyboard.Modifiers == ModifierKeys.Control ? 5 : 1;

            if (e.Delta > 0)
            {
                if (Value < Maximum)
                    Value = Convert.ToInt32(textBox.Text) + step;
            }
            else
            {
                if (Value > Minimum)
                    Value = Convert.ToInt32(textBox.Text) - step;
            }

            //textBox.Text = Value.ToString();
            e.Handled = true;
        }

        private void _textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            #region Changes the value of the Numeric Up and Down

            //var textBox = sender as TextBox;

            //if (textBox == null) return;
            //if (String.IsNullOrEmpty(textBox.Text)) return;

            //int newValue = Convert.ToInt32(textBox.Text);

            //if (newValue > Maximum)
            //    Value = Maximum;
            //else if (newValue < Minimum)
            //    Value = Minimum;
            //else
            //{
            //    Value = newValue;
            //}

            #endregion
        }

        private void PastingEvent(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                var text = (String)e.DataObject.GetData(typeof(String));

                if (IsTextDisallowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        #endregion

        #region Public Methods

        public void SelectAll()
        {
            _textBox?.SelectAll();
            _textBox?.Focus();
        }

        #endregion
    }
}
