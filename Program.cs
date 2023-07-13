using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

ApplicationConfiguration.Initialize();

var form = new Form();

form.WindowState = FormWindowState.Maximized;
form.FormBorderStyle = FormBorderStyle.None;

Graphics g = null;
Bitmap bmp = null;

PictureBox pb = new PictureBox();
pb.Dock = DockStyle.Fill;
form.Controls.Add(pb);

Point cursor = Point.Empty;
pb.MouseMove += (s, e) =>
{
    cursor = e.Location;
};

Queue<DateTime> queue = new Queue<DateTime>();
queue.Enqueue(DateTime.Now);

Image newImage = Image.FromFile("sonicSprites.png");

var posx = 400;
var posy = 400;

var last = DateTime.Now;

int moviment = 0;
int speed = 0;
int acceleration = 15;

int incrementSprite = 14;
var tm = new Timer();
tm.Interval = 20;
tm.Tick += delegate
{
    var now = DateTime.Now;
    queue.Enqueue(now);

    if ((now - last).TotalMilliseconds >= 5)
    {
        posx += speed;
        Rectangle destRect = new Rectangle(posx, posy, 105, 100);
        GraphicsUnit units = GraphicsUnit.Pixel;

        g.Clear(Color.White);
        last = now;
        float x = 11.8f + (48.2f * incrementSprite);
        float y = 234;
        float width = 42;
        float height = 40;

        g.DrawImage(newImage, destRect, x, y, width, height, units);

        incrementSprite--;

        if (incrementSprite <= 0)
            incrementSprite = 13;
    }

    if (queue.Count > 19)
    {
        DateTime old = queue.Dequeue();
        var time = now - old;
        var fps = (int)(19 / time.TotalSeconds);
        var drawFont = new Font("Arial", 16);
        PointF drawPoint = new PointF(150.0F, 150.0F);

        g.DrawString($"{fps} fps", drawFont, Brushes.Black, drawPoint);
    }

    pb.Refresh();
};

form.Load += delegate
{
    bmp = new Bitmap(pb.Width, pb.Height);
    g = Graphics.FromImage(bmp);
    g.Clear(Color.Red);
    pb.Image = bmp;
    tm.Start();
};
bool flag1 = false;
bool flag2 = false;

form.KeyPreview = true;
form.KeyPress += (s, e) =>
{
    if (!flag1 && (e.KeyChar == 'A' || e.KeyChar == 'a'))
    {
        flag1 = true;
        speed -= acceleration;
        moviment = 1;
    }
    if (!flag2 && (e.KeyChar == 'D' || e.KeyChar == 'd'))
    {
        flag2 = true;
        speed += acceleration;
        moviment = 2;
    }

    if (e.KeyChar == (char)Keys.Escape)
        Application.Exit();
};

form.KeyUp += (s, e) =>
{
    if(flag1 && e.KeyCode == Keys.A)
    {
        flag1 = false;
        speed += acceleration;
        moviment = 0;
    }
    if(flag2 && e.KeyCode == Keys.D)
    {
        flag2 = false;
        speed -= acceleration;
        moviment = 0;
    }
};

Application.Run(form);