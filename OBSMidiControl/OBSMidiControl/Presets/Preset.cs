using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBSMidiControl.Presets
{
    public class Preset
    {
        //public Scene Scene1
        //{
        //    get { return SceneArray[0]; }
        //    set { SceneArray[0] = value; }
        //}

        //public Scene Scene2
        //{
        //    get { return SceneArray[1]; }
        //    set { SceneArray[1] = value; }
        //}

        //public Scene Scene3
        //{
        //    get { return SceneArray[2]; }
        //    set { SceneArray[2] = value; }
        //}

        //public Scene Scene4
        //{
        //    get { return SceneArray[3]; }
        //    set { SceneArray[3] = value; }
        //}

        //public Scene Scene5
        //{
        //    get { return SceneArray[4]; }
        //    set { SceneArray[4] = value; }
        //}

        //public Scene Scene6
        //{
        //    get { return SceneArray[5]; }
        //    set { SceneArray[5] = value; }
        //}

        public Scene[] SceneArray;
        public string Name;

        private Master _master;

        public Master Master 
        {
            get { return _master; }
            set { _master = value; }
        }


        public Preset()
        {
            SceneArray = new Scene[6];
            Name = "emtpy";
            _master = new Master();
            for (int i = 0; i < 6; i++)
            {
                SceneArray[i] = new Scene();
            }
        }

        //public Preset(Scene scene1, Scene scene2, Scene scene3, Scene scene4, Scene scene5, Scene scene6)
        //{
        //    SceneArray = new Scene[6] { Scene1, Scene2, Scene3, Scene4, Scene5, Scene6 };
        //}

        public Preset(string name, Scene[] sceneArray)
        {
            Name = name;
            SceneArray = sceneArray;
            _master = new Master();
            //for (int i = 0; i < Math.Min(sceneArray.Length, 6); i++)
            //{
            //    SceneArray[i] = sceneArray[i];
            //}
        }

    }
}
