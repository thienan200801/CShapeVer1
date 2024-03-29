﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;


namespace Ver1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Width = 1100;
            this.Height = 700;
            bm = new Bitmap(pic.Width, pic.Height);
            g = Graphics.FromImage(bm);
            g.Clear(Color.White);
            pic.Image = bm;
            if(Convert.ToInt32(lineSize.Text) < 1)
                p = new Pen(Color.Black, Convert.ToInt32(lineSize.Text));
            else p = new Pen(Color.Black, Convert.ToInt32(lineSize.Text));
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (FontFamily font in FontFamily.Families)
            {
                FontBox.Items.Add(font.Name.ToString());
            }
            FontBox.SelectedItem = "Arial";
            for(int i = 1; i < 100; i++)
            {
                TextSizeBox.Items.Add(i.ToString());
            }
            TextSizeBox.SelectedItem = "8";
        }

        Bitmap bm; 
        Graphics g; 
        bool paint = false; 
        Point px, py; 
        Pen p = new Pen(Color.Black, 1); 
        Pen eraser = new Pen(Color.White, 5); 
        int index; 
        int x, y, sX, sY, cX, cY;
        ColorDialog cd = new ColorDialog();
        Color new_color = Color.Black;
        Image file;
        Font fontSelection = new Font("Arial", 1);
        SolidBrush drawBrush = new SolidBrush(Color.Black);
        Boolean opened = false;
        int Size = 1;

        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            paint = true; 
            py = e.Location;

            cX = e.X;
            cY = e.Y;
        }


        private void pic_MouseMove(object sender, MouseEventArgs e)
        {
            if (paint)
            {
                if (index == 1) 
                {
                    px = e.Location;
                    g.DrawLine(p, px, py);
                    py = px;
                }
                else if (index == 2)
                {
                    px = e.Location;
                    g.DrawLine(eraser, px, py);
                    py = px;
                }
            }
            pic.Refresh();

            x = e.X;
            y = e.Y;
            sX = e.X - cX;
            sY = e.Y - cY;
        }

        private void pic_MouseUp(object sender, MouseEventArgs e)
        {
            paint = false;

            sX = x - cX;
            sY = y - cY;

            if (index == 3)
            {
                g.DrawEllipse(p, cX, cY, sX, sY);
            }
            if (index == 4)
            {
                g.DrawRectangle(p, cX, cY, sX, sY);
            }
            if (index == 5)
            {
                g.DrawLine(p, cX, cY, x, y);
            }
            else if (index == 8)
            {
                DrawText(x, y);
            }
        }



        private void pic_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            p = new Pen(new_color, Convert.ToInt32(lineSize.Text));
            if (paint)
            {
                if (index == 3)
                {
                    g.DrawEllipse(p, cX, cY, sX, sY);
                }
                else if (index == 4)
                {
                    g.DrawRectangle(p, cX, cY, sX, sY);
                }
                else if (index == 5)
                {
                    g.DrawLine(p, cX, cY, x, y);
                }
                else if (index == 8)
                {
                    DrawText(x, y);
                }
            }

        }

        private void newPage_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            pic.Image = bm;
            index = 0;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Image(*.jpg)|*.jpg|(*.*|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Bitmap btm = bm.Clone(new Rectangle(0, 0, pic.Width, pic.Height), bm.PixelFormat);
                btm.Save(sfd.FileName, ImageFormat.Jpeg);
                MessageBox.Show("Photo saved!");
            }
        }

        static Point set_point(PictureBox pb, Point pt)
        {
            float pX = 1f * pb.Image.Width / pb.Width;
            float pY = 1f * pb.Image.Height / pb.Height;
            return new Point((int)(pt.X * pX), (int)(pt.Y * pY));
        }

        private void colorPicker_MouseClick(object sender, MouseEventArgs e)
        {
            Point point = set_point(color_picker, e.Location);
            pic_color.BackColor = ((Bitmap)color_picker.Image).GetPixel(point.X, point.Y);
            new_color = pic_color.BackColor;
            p.Color = pic_color.BackColor;

        }

        private void validate(Bitmap bm, Stack<Point> sp, int x, int y, Color old_color, Color new_color)
        {
            Color cx = bm.GetPixel(x, y);
            if (cx == old_color)
            {
                sp.Push(new Point(x, y));
                bm.SetPixel(x, y, new_color);
            }
        }

        public void Fill(Bitmap bm, int x, int y, Color new_clr)
        {
            Color old_color = bm.GetPixel(x, y);
            Stack<Point> pixel = new Stack<Point>();
            pixel.Push(new Point(x, y));
            bm.SetPixel(x, y, new_clr);
            if (old_color == new_clr) return;

            while (pixel.Count > 0)
            {
                Point pt = (Point)pixel.Pop();
                if (pt.X > 0 && pt.Y > 0
                    && pt.X < bm.Width - 1
                    && pt.Y < bm.Height - 1)
                {
                    validate(bm, pixel, pt.X - 1, pt.Y, old_color, new_clr);
                    validate(bm, pixel, pt.X, pt.Y - 1, old_color, new_clr);
                    validate(bm, pixel, pt.X + 1, pt.Y, old_color, new_clr);
                    validate(bm, pixel, pt.X, pt.Y + 1, old_color, new_clr);

                }
            }
        }


        private void pic_Click(object sender, MouseEventArgs e)
        {
            if (index == 7)
            {
                Point point = set_point(pic, e.Location);
                Fill(bm, point.X, point.Y, new_color);
            }

        }

        private void btn_color_Click_1(object sender, EventArgs e)
        {
            cd.ShowDialog();
            new_color = cd.Color;
            pic_color.BackColor = cd.Color;
            p.Color = cd.Color;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dr = new OpenFileDialog();
            dr.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";

            if (dr.ShowDialog() == DialogResult.OK)
            {
                file = Image.FromFile(dr.FileName);
                try
                {
                    if(file != null)
                    pic.Image = file;
                    opened = true;
                    
                }
                catch
                {
                    MessageBox.Show("Sorry, something when wrong. Please try again");
                }
            }
        }

        private void DrawText(int x, int y)
        {
        int SizeFont = 8;
            Font myFont = new System.Drawing.Font(FontBox.SelectedItem.ToString(), (float)Convert.ToDouble(TextSizeBox.SelectedItem), FontStyle.Italic);

            Brush myBrush = new SolidBrush(new_color);
            string str = txt.Text;

            g.DrawString(str, myFont, myBrush, x, y);
        }

        private void effectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEffect newEffect = new AddEffect();
            newEffect.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void TextSizeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Size = Int32.Parse(TextSizeBox.Text);
            p.Width = Size;
        }


        private void btn_pencil_Click(object sender, EventArgs e)
        {
            index = 1;
        }

        private void inc_Click(object sender, EventArgs e)
        {
            int temp = int.Parse(lineSize.Text);
            lineSize.Text = (temp+1).ToString();
        }

        private void dec_Click(object sender, EventArgs e)
        {
            int temp = int.Parse(lineSize.Text);
                if(temp > 1) lineSize.Text = (temp - 1).ToString();
                else MessageBox.Show("Cannot decrease when size equal to 1");            
        }

        private void FontBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btn_eraser_Click(object sender, EventArgs e)
        {
            index = 2;
        }
        private void btn_ellipse_Click(object sender, EventArgs e)
        {
            index = 3;
        }
        private void btn_rect_Click(object sender, EventArgs e)
        {
            index = 4;
        }
        private void btn_line_Click(object sender, EventArgs e)
        {
            index = 5;
        }

        private void btn_fill_Click(object sender, EventArgs e)
        {
            index = 7;
        }
        private void btn_text_Click_1(object sender, EventArgs e)
        {
            index = 8;
        }


    }
}
