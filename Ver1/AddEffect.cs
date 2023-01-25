using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Ver1
{
    public partial class AddEffect : Form
    {
        public AddEffect()
        {
            InitializeComponent();
            this.Width = 800;
            this.Height = 600;
            mainPic.Width = 700;
            mainPic.Height = 350;
            bm = new Bitmap(mainPic.Width, mainPic.Height);
            g = Graphics.FromImage(bm);
            g.Clear(Color.White);
            mainPic.Image = bm;
        }
        Bitmap bm;
        Graphics g;

        //read file
        Image file;
        Boolean opened = false;

        public void reload()
        {
            if (!opened)
            {
                MessageBox.Show("Open and then apply effect");
            }
            else
            {
                if (opened)
                {
                    file = Image.FromFile(openFileDialog1.FileName);
                    mainPic.Image = file;
                    opened = true;
                }
            }
        }
        
        public void filter2(float[][] data)
        {
            if (!opened)
            {
                MessageBox.Show("Open please");
            }
            else
            {
                Image img = mainPic.Image;
                Bitmap bmpInverted = new Bitmap(img.Width, img.Height);

                ImageAttributes ia = new ImageAttributes();
                ColorMatrix cmPicture = new ColorMatrix(data);
                ia.SetColorMatrix(cmPicture);
                Graphics g = Graphics.FromImage(bmpInverted);
                g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);
                g.Dispose();
                mainPic.Image = bmpInverted;
            }
        }

        void Hue()
        {
            float changered = red_trackBar.Value * 0.1f; 
            float changegreen = green_trackBar.Value * 0.1f;
            float changeblue = blue_trackBar.Value * 0.1f;

            red_trackBar.Text = changered.ToString();
            green_trackBar.Text = changeblue.ToString();
            blue_trackBar.Text = changegreen.ToString();

            reload();
            if (!opened)
            {
                MessageBox.Show("Open please");
            }
            else
            {
                Image img = mainPic.Image;                             
                Bitmap bmpInverted = new Bitmap(img.Width, img.Height);   

                ImageAttributes ia = new ImageAttributes();                 
                ColorMatrix cmPicture = new ColorMatrix(new float[][]      
                {
                    new float[]{1+changered, 0, 0, 0, 0},
                    new float[]{0, 1+changegreen, 0, 0, 0},
                    new float[]{0, 0, 1+changeblue, 0, 0},
                    new float[]{0, 0, 0, 1, 0},
                    new float[]{0, 0, 0, 0, 1}
                });
                ia.SetColorMatrix(cmPicture);           
                Graphics g = Graphics.FromImage(bmpInverted);   

                g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);


                g.Dispose();                           
                mainPic.Image = bmpInverted;
            }
        }


        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            mainPic.Image = bm;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Image(*.jpg)|*.jpg|(*.*|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Bitmap btm = bm.Clone(new Rectangle(0, 0, mainPic.Width, mainPic.Height), bm.PixelFormat);
                btm.Save(sfd.FileName, ImageFormat.Jpeg);
                MessageBox.Show("Photo saved!");
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();

            if (dr == DialogResult.OK)
            {
                file = Image.FromFile(openFileDialog1.FileName);
                mainPic.Image = file;
                opened = true;
            }
        }

        private void b1_Click(object sender, EventArgs e)
        {
            reload();
            red_trackBar.Value = 0;
            green_trackBar.Value = 0;
            blue_trackBar.Value = 0;
        }


        private void red_track(object sender, EventArgs e)
        {
            Hue();
        }

        private void green_track(object sender, EventArgs e)
        {
            Hue();
        }

        private void blue_track(object sender, EventArgs e)
        {
            Hue();
        }
        private void b2_Click(object sender, EventArgs e)
        {
            reload();
            filter2(
                new float[][]
                {
                    new float[] {.393f, .349f+0.5f, .272f, 0, 0},
                    new float[] {.769f+0.3f, .686f, .534f, 0, 0},
                    new float[] {.189f, .168f, .131f+0.5f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1},
                }
                );
        }

        private void b3_Click(object sender, EventArgs e)
        {
            reload();
            filter2(
                new float[][]
                {
                    new float[]{.393f, .349f, .272f, 0, 0},
                    new float[]{.769f, .686f, .534f, 0, 0},
                    new float[]{.189f, .168f, .131f, 0, 0},
                    new float[]{0, 0, 0, 1, 0},
                    new float[]{0, 0, 0, 0, 1}
                }
                );
        }

        private void b4_Click(object sender, EventArgs e)
        {
            reload();
            filter2(
                new float[][]
                {
                    new float[]{0.299f, 0.299f, 0.299f, 0, 0},
                    new float[]{0.587f, 0.587f, 0.587f, 0, 0},
                    new float[]{0.114f, 0.114f, 0.114f, 0, 0},
                    new float[]{0, 0, 0, 1, 0},
                    new float[]{0, 0, 0, 0, 0}
                }
                );
        }

        private void b5_Click(object sender, EventArgs e)
        {
            reload();
            filter2(
                new float[][]
                {
                    new float[]{1+0.3f, 0, 0, 0, 0},
                    new float[]{0, 1+0f, 0, 0, 0},
                    new float[]{0, 0, 1+5f, 0, 0},
                    new float[]{0, 0, 0, 1, 0},
                    new float[]{0, 0, 0, 0, 1}
                }
                );
        }

        private void b6_Click(object sender, EventArgs e)
        {
            reload();
            filter2(
                new float[][]
                {
                    new float[]{1+0.9f, 0, 0, 0, 0},
                    new float[]{0, 1+1.5f, 0, 0, 0},
                    new float[]{0, 0, 1+1.3f, 0, 0},
                    new float[]{0, 0, 0, 1, 0},
                    new float[]{0, 0, 0, 0, 1}
                }
                );
        }

        private void b7_Click(object sender, EventArgs e)
        {
            reload();
            filter2(
                new float[][]
                {
                    new float[]{1+0.3f, 0, 0, 0, 0},
                    new float[]{0, 1+0.7f, 0, 0, 0},
                    new float[]{0, 0, 1+1.3f, 0, 0},
                    new float[]{0, 0, 0, 1, 0},
                    new float[]{0, 0, 0, 0, 1}
                }
                );
        }

        private void b8_Click(object sender, EventArgs e)
        {
            reload();
            filter2(
                new float[][]
                {
                    new float[]{1,0,0,0,0},
                    new float[]{0,1,0,0,0},
                    new float[]{0,0,1,0,0},
                    new float[]{0, 0, 0, 1, 0},
                    new float[]{0, 0, 1, 0, 1}
                }
                );
        }

        private void b9_Click(object sender, EventArgs e)
        {
            reload();
            filter2(
                new float[][]
                {
                    new float[]{.393f+0.3f, .349f, .272f, 0, 0},
                    new float[]{.769f, .686f+0.2f, .534f, 0, 0},
                    new float[]{.189f, .168f, .131f+0.9f, 0, 0},
                    new float[]{0, 0, 0, 1, 0},
                    new float[]{0, 0, 0, 0, 1}
                }
                );
        }

        private void b10_Click(object sender, EventArgs e)
        {
            reload();
            filter2(
                new float[][]
                {
                    new float[] {.38f, .349f, .24f, 0, 0},
                    new float[] {.769f, .624f, .534f, 0, 0},
                    new float[] {.189f, .100f, .131f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1},
                }
                );
        }

        private void b11_Click(object sender, EventArgs e)
        {
            reload();
            filter2(
                new float[][]
                {
                    new float[] {.3f, .4f+0.5f, .2f, 0, 0},
                    new float[] {.5f, .6f, .534f, 0, 0},
                    new float[] {.1f, .1f, .1f+0.5f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1},
                }
                );
        }

        private void b12_Click(object sender, EventArgs e)
        {
            reload();
            filter2(
                new float[][]
                {
                    new float[] {.5f, .35f, .27f, 0, 0},
                    new float[] {.73f, .686f+.33f, .54f, 0, 0},
                    new float[] {.189f, .168f, .135f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1},
                }
                );
        }

        private void b14_Click(object sender, EventArgs e)
        {
            reload();
            filter2(
                new float[][]
                {
                    new float[] {.393f, .349f+0.5f, .272f, 0, 0},
                    new float[] {.769f+0.3f, .686f, .534f, 0, 0},
                    new float[] {.189f, .168f, .131f+0.5f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1},
                }
                );
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void red_trackBar_Scroll(object sender, EventArgs e)
        {

        }

    }
}
