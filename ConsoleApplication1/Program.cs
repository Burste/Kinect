using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
/*
 以C#設計一個以聲音定位控制Kienct垂直角度的應用程式，需符合下列條件：

 * 可調整角度為：歸零、+/- 5、 +/- 10、最大角度、最小角度。
音量需超過90%才進行調整。
 * 
使用者介面僅顯示角度調整使用方式、目前角度、使用者欲調整角度、調整後角度。

 */

namespace ConsoleApplication1
{
    class Program
    {
        static KinectAudioSource kinectaudiosource;
        static void Main(string[] args)
        {
            KinectSensor sensor = KinectSensor.KinectSensors[0];
            sensor.Start();
            kinectaudiosource = sensor.AudioSource;

            SoundTracking();
            Console.WriteLine("請按下空白建結束");
            if (Console.ReadKey().Key != ConsoleKey.Spacebar)
            {
                sensor.ElevationAngle = 0;
            }
            
            sensor.Stop();
        }
        //聲音偵測
        static void SoundTracking()
        {
            KinectAudioSource audioSource = AudioSourceSetup();

            audioSource.BeamAngleChanged += audioSource_BeamAngleChanged;
            audioSource.SoundSourceAngleChanged += audioSource_SoundSourceAngleChanged;

            audioSource.Start();
        }

        //聲音參數設定
        static KinectAudioSource AudioSourceSetup()
        {
            KinectAudioSource source = kinectaudiosource;
            source.NoiseSuppression = true;
            source.AutomaticGainControlEnabled = true;
            source.BeamAngleMode = BeamAngleMode.Adaptive;
            return source;
        }
        //取得麥克風陣列最新對準方向
        static void audioSource_BeamAngleChanged(object sender, BeamAngleChangedEventArgs e)
        {
            //string maxmin = " ,最大Beam Angle :" + KinectAudioSource.MaxBeamAngle
            //               + " , 最小Beam Angle :" + KinectAudioSource.MinBeamAngle;
            //string output = "偵測到Beam Angle :" + e.Angle.ToString() + maxmin;
            //Console.WriteLine(output);
        }
        //來源方向
        static void audioSource_SoundSourceAngleChanged(object sender, SoundSourceAngleChangedEventArgs e)
        {
            KinectSensor sensor = KinectSensor.KinectSensors[0];
            sensor.Start();

            string maxmins = " ,最大Source Angle :" + KinectAudioSource.MaxSoundSourceAngle
                                        + " , 最小Sound Angle :" + KinectAudioSource.MinSoundSourceAngle;

            string maxmin = " ,最大Beam Angle :" + KinectAudioSource.MaxBeamAngle
                           + " , 最小Beam Angle :" + KinectAudioSource.MinBeamAngle;
           
            //+ maxmin;
           
            
            if (e.ConfidenceLevel >= 0.9)
            {


                string output = "來源角度 :" + e.Angle.ToString("#.##")
                   + " , Source Confidence: " + e.ConfidenceLevel.ToString("#.##");
                Console.WriteLine(output);
                    if ((int)e.Angle > 0)
                    {
                        if ((sensor.ElevationAngle <= 22) )
                        { sensor.ElevationAngle += 5; Console.Write("硬了硬了~~~~~~~~\n"); }
                    }
                    if ((int)e.Angle < 0)
                    {
                        if ( (sensor.ElevationAngle >= -22))
                        { sensor.ElevationAngle -= 5; Console.Write("軟掉啦~~~~~~~~\n"); }
                    }
               

                Console.WriteLine(output);
            }

        }

    }
}
