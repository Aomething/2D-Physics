//File: DrawLine.cs
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Media;
using System.Numerics;
using System.Security.Policy;
using System.CodeDom;
using System.Security.AccessControl;
using System.Collections.Generic;
using System.Windows.Forms.VisualStyles;
using Microsoft.VisualBasic;

public class Body{
    public string Name;
    public bool X_Motion = true;
    public bool Y_Motion = true;
    public float X_pos = 0;
    public float Y_pos = 0;
    public float X_init = 0;
    public float Y_init = 0;
    public float X_Vel = 0;
    public float Y_Vel = 0;
    public float Mass = 0;
    public float width = 1;
    public float height = 1;
    public float radius = 1;
    public bool isSelected = false;
    public SolidBrush col;
    public Body(string n, bool xm, bool ym, float xp, float yp, float w, float h, float r, float xi, float yi, float xv, float yv, float m, bool sel, SolidBrush c) {
        Name = n;
        X_Motion = xm;
        Y_Motion = ym;
        X_pos = xp;
        Y_pos = yp;
        width = w;
        height = h;
        radius = r;
        X_init = xi;
        Y_init = yi;
        X_Vel = xv;
        Y_Vel = yv;
        Mass = m;
        isSelected = sel;
        col = c;
    }
}



public class MainForm : System.Windows.Forms.Form
{

    
    public SolidBrush[] colorList = new SolidBrush[10];     
    SolidBrush brush0 = new SolidBrush(Color.Red);
    SolidBrush brush1 = new SolidBrush(Color.Blue);
    SolidBrush brush2 = new SolidBrush(Color.Green);
    SolidBrush brush3 = new SolidBrush(Color.Yellow);
    SolidBrush brush4 = new SolidBrush(Color.Orange);
    SolidBrush brush5 = new SolidBrush(Color.Black);
    SolidBrush brush6 = new SolidBrush(Color.Purple);
    SolidBrush brush7 = new SolidBrush(Color.Pink);
    SolidBrush brush8 = new SolidBrush(Color.Gray);
    SolidBrush brush9 = new SolidBrush(Color.Brown);
    
    static int objectCount = 10;
    public Body[] objectList = new Body[objectCount];
    Random rand = new Random();
    public void initObjects() {
        colorList[0] = brush0;
        colorList[1] = brush1;
        colorList[2] = brush2;
        colorList[3] = brush3;
        colorList[4] = brush4;
        colorList[5] = brush5;
        colorList[6] = brush6;
        colorList[7] = brush7;
        colorList[8] = brush8;
        colorList[9] = brush9;
        for (int i = 0; i < objectCount; i++) {
            int colorIndex = rand.Next(colorList.Length);
            objectList[i] = new Body("circle", true, true, rand.Next(200), rand.Next(200), 0, 0, rand.Next(15, 25), rand.Next(200), 
            rand.Next(200), rand.Next(-30, 30), 0, 15, false, colorList[colorIndex]);
        }
        objectList[0].isSelected = true;
    }
    
    
    
    //objectList[0] = new Body("circle", true, true, 100, 100, 0, 0, 25, 100, 100, 10, 0, 15, false);
    public float accel = 5;

    private void OnPaint(Object sender, PaintEventArgs e) {

        Pen highlight = new Pen(Color.Black);
        highlight.Width = 2;
        
        // draw circle
        for (int i = 0; i < objectCount; i++) {
            Rectangle rect = new Rectangle();
            rect.Width = (int)objectList[i].radius * 2;
            rect.Height = (int)objectList[i].radius * 2;
            rect.X = (int)objectList[i].X_pos - (int)objectList[i].radius;
            rect.Y = (int)objectList[i].Y_pos - (int)objectList[i].radius;
            e.Graphics.FillEllipse(objectList[i].col, objectList[i].X_pos - objectList[i].radius, objectList[i].Y_pos - objectList[i].radius, objectList[i].radius * 2,  objectList[i].radius * 2);
            
            // if a particular object is selected, highlight it with its bounding box
            if (objectList[i].isSelected == true) {
                e.Graphics.DrawRectangle(highlight, rect);
            }
        }        
    }

    private void OnKeyDown(Object sender, KeyEventArgs e) {
        // handle arrow key presses to move object
        for (int i = 0; i < objectCount; i++) {
            switch(e.KeyCode) {
                case Keys.Left:
                    objectList[i].X_pos -= 5;
                break;

                case Keys.Right:
                    objectList[i].X_pos += 5;
                break;

                case Keys.Up:
                    objectList[i].Y_pos -= 5;
                break;

                case Keys.Down:
                    objectList[i].Y_pos += 5;
                break;
            }
        }
        //redraw and/or update
        this.Invalidate();
    }

    private void OnMouseDown(Object sender, System.Windows.Forms.MouseEventArgs e) {
        switch (e.Button) {
            case MouseButtons.Left:
                objectList[0].X_pos = e.X - objectList[0].radius;
                objectList[0].Y_pos = e.Y - objectList[0].radius;
                objectList[0].Y_Vel = 0;
                objectList[0].Y_Motion = true;
            break;
        }
        
    }

    private void OnTimerClick(Object sender, EventArgs e) {
       
        for (int i = 0; i < objectCount; i++) {
            //distance = Math.Sqrt((init_x - init_x2) * (init_x - init_x2) + (init_y - init_y2) * (init_y - init_y2));
            if (objectList[i].X_Motion) {
                objectList[i].X_pos += objectList[i].X_Vel;
            }

            if (objectList[i].Y_Motion) {
                objectList[i].Y_pos += objectList[i].Y_Vel;
                objectList[i].Y_Vel += accel;
            }
            else {
                if (objectList[i].Y_pos + objectList[i].radius <= this.Height - 60) {
                    objectList[i].Y_Motion = true;
                }
            }

            // if collide with bottom, remove part of velocity, invert direction
        
            if (objectList[i].Y_pos + objectList[i].radius > this.Height - 45) {
                objectList[i].Y_pos = this.Height - objectList[i].radius - 45;
                if (objectList[i].Y_Vel >= 30 || objectList[i].Y_Vel <= -30) {
                    objectList[i].Y_Vel *= -72;
                    objectList[i].Y_Vel /= 100;
                }
                else {
                    objectList[i].Y_Vel *= -50;
                    objectList[i].Y_Vel /= 100;
                }   
                
                if (objectList[i].Y_Vel <= 22 && objectList[i].Y_Vel >= -22) {
                    objectList[i].Y_Vel = 0;
                    objectList[i].Y_Motion = false;
                }
            }
            // if collide with sides, remove part of velocity, invert direction
            // right side
            if (objectList[i].X_pos + objectList[i].radius * 2 >= this.Width) {
                objectList[i].X_pos = this.Width - objectList[i].radius * 2- 1;
                objectList[i].X_Vel *= -80;
                objectList[i].X_Vel /= 100;
                if (objectList[i].X_Vel <= 2 && objectList[i].X_Vel >= -2) {
                    objectList[i].X_Vel = 0;
                    objectList[i].X_Motion = false;
                }    
            }



            // left side
            if (objectList[i].X_pos - objectList[i].radius <= 0) {
                objectList[i].X_pos = objectList[i].radius + 1;
                objectList[i].X_Vel *= -80;
                objectList[i].X_Vel /= 100;
                if (objectList[i].X_Vel <= 2 && objectList[i].X_Vel >= -2) {
                    objectList[i].X_Vel = 0;
                    objectList[i].X_Motion = false;
                }
            }

            this.Invalidate();
        }
    }

    private System.ComponentModel.Container components;
    public MainForm()
    {
        InitializeComponent();
        initObjects();
        CenterToScreen();
        SetStyle(ControlStyles.ResizeRedraw, true);
    }
    //  Clean up any resources being used.
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (components != null)
            {
                components.Dispose();
            }
        }
        base.Dispose(disposing);
    }
    #region Windows Form Designer generated code
    private void InitializeComponent()
    {
        // animation stuff
        this.DoubleBuffered = true;
        this.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);
        this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);

        // page setup
        this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
        this.ClientSize = new System.Drawing.Size(500, 500);
        this.Text = "Physics Testing";
        this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
        this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);

        // timer to repaint object
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        //timer.Interval = 8; // 120 fps
        //timer.Interval = 16; // 60 fps
        timer.Interval = 33; // 30 fps
        //timer.Interval = 67; // 15 fps
        //timer.Interval = 500; // 2 fps
        timer.Tick += new EventHandler(this.OnTimerClick);
        timer.Start();
    }
    
    #endregion
    [STAThread]
    static void Main()
    {
        Application.Run(new MainForm());    
    }
    private void MainForm_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
    {
        Graphics g = this.CreateGraphics();
        Pen p = new Pen(Color.FromArgb(-16776961), 7);  // draw the line
    }
}

public static class GraphicsExtensions {
    public static void DrawCircle(this Graphics g, Pen pen, float centerX, float centerY, float radius) {
        g.DrawEllipse(pen, centerX - radius, centerY - radius, radius*2, radius*2);        
    }

    public static void FillCircle(this Graphics g, Brush brush, float centerX, float centerY, float radius) {
        g.FillEllipse(brush, centerX - radius, centerY - radius, radius*2, radius*2);
    }
}

