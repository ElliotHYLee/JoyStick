﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SlimDX.DirectInput;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        DirectInput Input = new DirectInput();
        SlimDX.DirectInput.Joystick stick;
        Joystick[] Sticks;
        Joystick stick1;
        //Thumstick variables.
        int yValue = 0;
        int xValue = 0;
        int zValue = 0;
        int rotationZValue = 0;
        int rotationXValue = 0;
        int rotationYValue = 0;
        bool isFirst= true;

        public Form1()
        {
            InitializeComponent();
            GetSticks();
            Sticks = GetSticks();
            stick1 = Sticks[0];
            timer1.Enabled = true;
            timer1.Interval = 1;
            isFirst = true;
            //Console.WriteLine("here");
            while(true)
            {
                StickHandlingLogic(stick1, 0);
            }

        }


        private void Form1_Load(object sender, EventArgs e)
        {
            Joystick[] joystick = GetSticks();
        }

        void StickHandlingLogic(Joystick stick, int id)
        {
            //Console.WriteLine("here2");
            // Creates an object from the class JoystickState.
            JoystickState state = new JoystickState();
            
            state = stick.GetCurrentState(); //Gets the state of the joystick
            //Console.WriteLine(state);
            //These are for the thumbstick readings
            yValue = -state.Y;
            xValue = state.X;
            zValue = state.Z;
            rotationZValue = state.RotationZ;
            rotationXValue = state.RotationY;
            rotationYValue = state.RotationX;

            int th = 0;
            int[] z = state.GetSliders();
            th= z[0];
            if (z[0] == 0 && isFirst)
            {
                th = 0;
                   
            }
            else
            {
                if(isFirst) isFirst = false;
                if (th >= 0)
                {
                    th = 50 - th / 2;
                }
                else
                {
                    th = -th / 2 + 50;
                }
            }


            Console.Write("thrust = " + th);
            
            

            Console.WriteLine(" x = " + xValue + " y = " + yValue + " z = " + zValue + " rot x = " + rotationXValue + " rot y = " + rotationYValue + " rot Z = " + rotationZValue);

            bool[] buttons = state.GetButtons(); // Stores the number of each button on the gamepad into the bool[] butons.
           // Console.WriteLine("# of button = " + buttons.Length);
            //Here is an example on how to use this for the joystick in the first index of the array list
            if (id == 0)
            {
                // This is when button 0 of the gamepad is pressed, the label will change. Button 0 should be the square button.
                if (buttons[0])
                {
                   
                }

            }
        }

        public Joystick[] GetSticks()
        {

            List<SlimDX.DirectInput.Joystick> sticks = new List<SlimDX.DirectInput.Joystick>(); // Creates the list of joysticks connected to the computer via USB.
            
            foreach (DeviceInstance device in Input.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly))
            {
                // Creates a joystick for each game device in USB Ports
                try
                {
                    stick = new SlimDX.DirectInput.Joystick(Input, device.InstanceGuid);
                    stick.Acquire();

                    // Gets the joysticks properties and sets the range for them.
                    foreach (DeviceObjectInstance deviceObject in stick.GetObjects())
                    {
                        if ((deviceObject.ObjectType & ObjectDeviceType.Axis) != 0)
                            stick.GetObjectPropertiesById((int)deviceObject.ObjectType).SetRange(-100, 100);
                    }

                    // Adds how ever many joysticks are connected to the computer into the sticks list.
                    sticks.Add(stick);
                }
                catch (DirectInputException)
                {
                }
            }
            Console.WriteLine(sticks.Count);
            return sticks.ToArray();
        }

        //Creates the StickHandlingLogic Method which takes all the joysticks in the sticks List and puts them into a timer.
        public void timer1_Tick(object sender, EventArgs e)
        {
            //Console.WriteLine("here333");
            for (int i = 0; i < Sticks.Length; i++)
            {
                StickHandlingLogic(Sticks[i], i);
            }
        }
    }
}
