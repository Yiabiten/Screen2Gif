﻿using System;
using System.Drawing;
using System.Windows.Forms;
using ScreenToGif.Properties;

namespace ScreenToGif.Pages
{
    /// <summary>
    /// Free Drawing form.
    /// </summary>
    public partial class FreeDrawing : Form
    {
        #region Variables

        private bool _paint;

        #endregion

        /// <summary>
        /// Default constructor of the FreeDraw form.
        /// </summary>
        public FreeDrawing(Image background, ImageLayout layout, Size imageSize)
        {
            InitializeComponent();

            panelDrawing.BackgroundImage = background;
            panelDrawing.BackgroundImageLayout = layout;

            if (layout == ImageLayout.Tile)
            {
                panelDrawing.Width = imageSize.Width;
                panelDrawing.Height = imageSize.Height;

                BackgroundImage = null;
            }
            else
            {
                panelDrawing.Width = background.Width;
                panelDrawing.Height = background.Height;
            }

            //TODO: Get the real values.
            Width = panelDrawing.Width + 40;
            Height = panelDrawing.Height + 66 + 20;

            if (Width < 400) Width = 400;
            if (Height < 215) Height = 215;

            panelDrawing.EraseAll();

            #region Localize Labels

            Text = Resources.Title_FreeDrawing;
            lblBrushSize.Text = Resources.Label_BrushSize;
            lblEraserSize.Text = Resources.Label_EraserSize;

            #endregion
        }

        /// <summary>
        /// The Image generated by the user.
        /// </summary>
        public Bitmap ImagePainted { get; set; }

        #region Load/Closing

        private void FreeDraw_Load(object sender, EventArgs e)
        {
            numBrush.Value = trackBrush.Value = Settings.Default.freeDrawBrush;
            numEraser.Value = trackEraser.Value = Settings.Default.freeDrawEraser;

            colorDialog.Color = pbColor.BackColor = Settings.Default.freeDrawColor;
        }

        private void FreeDraw_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.freeDrawBrush = trackBrush.Value;
            Settings.Default.freeDrawEraser = trackEraser.Value;
            Settings.Default.freeDrawColor = pbColor.BackColor;
            Settings.Default.Save();
        }

        #endregion

        #region Button Events

        private void btnClear_Click(object sender, EventArgs e)
        {
            panelDrawing.EraseAll();
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            panelConfig.Visible = !panelConfig.Visible;
        }

        private void pbColor_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                pbColor.BackColor = colorDialog.Color;
            }
        }

        private void cbCircle_Click(object sender, EventArgs e)
        {
            cbCircle.Checked = true;
            cbSquare.Checked = false;
            cbEraser.Checked = false;

            panelConfig.Visible = false;
        }

        private void cbSquare_Click(object sender, EventArgs e)
        {
            cbCircle.Checked = false;
            cbSquare.Checked = true;
            cbEraser.Checked = false;

            panelConfig.Visible = false;
        }

        private void cbEraser_Click(object sender, EventArgs e)
        {
            cbCircle.Checked = false;
            cbSquare.Checked = false;
            cbEraser.Checked = true;
            _paint = false;

            panelConfig.Visible = false;
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            ImagePainted = panelDrawing.CachedBitmap;
            DialogResult = DialogResult.OK;
        }

        #endregion

        #region Config Events

        private void trackBrush_ValueChanged(object sender, EventArgs e)
        {
            numBrush.Value = trackBrush.Value;
        }

        private void trackEraser_ValueChanged(object sender, EventArgs e)
        {
            numEraser.Value = trackEraser.Value;
        }

        private void numBrush_ValueChanged(object sender, EventArgs e)
        {
            trackBrush.Value = Convert.ToInt32(numBrush.Value);
        }

        private void numEraser_ValueChanged(object sender, EventArgs e)
        {
            trackEraser.Value = Convert.ToInt32(numEraser.Value);
        }

        #endregion

        #region Drawing Events

        private void panelDrawing_MouseUp(object sender, MouseEventArgs e)
        {
            _paint = false;
        }

        private void panelDrawing_MouseDown(object sender, MouseEventArgs e)
        {
            panelConfig.Visible = false;
            _paint = true;

            var position = GetSpecificMousePosition(e);

            if (cbCircle.Checked)
            {
                panelDrawing.Draw(g => EllipseDraw(g, position));
            }
            else if (cbSquare.Checked)
            {
                panelDrawing.Draw(g => RectangleDraw(g, position));
            }
            else if (cbEraser.Checked)
            {
                panelDrawing.EraseEvent(e, trackEraser.Value);
            }
        }

        private void panelDrawing_MouseMove(object sender, MouseEventArgs e)
        {
            var position = GetSpecificMousePosition(e);

            if (_paint)
            {
                if (cbCircle.Checked)
                {
                    panelDrawing.Draw(g => EllipseDraw(g, position));
                }
                else if (cbSquare.Checked)
                {
                    panelDrawing.Draw(g => RectangleDraw(g, position));
                }
                else if (cbEraser.Checked)
                {
                    panelDrawing.EraseEvent(e, trackEraser.Value);
                }
            }
        }

        private void RectangleDraw(Graphics g, Point position)
        {
            g.FillRectangle(new SolidBrush(pbColor.BackColor), position.X, position.Y, trackBrush.Value, trackBrush.Value);
        }

        private void EllipseDraw(Graphics g, Point position)
        {
            g.FillEllipse(new SolidBrush(pbColor.BackColor), position.X, position.Y, trackBrush.Value, trackBrush.Value);
        }

        private Point GetSpecificMousePosition(MouseEventArgs e)
        {
            int xy = (trackBrush.Value/2);
            var position = new Point(e.X - xy, e.Y - xy);
            return position;
        }

        #endregion
    }
}
