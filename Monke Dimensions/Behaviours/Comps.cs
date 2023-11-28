using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monke_Dimensions.Interaction;
using UnityEngine;
using UnityEngine.UI;

namespace Monke_Dimensions.Behaviours
{
    internal class Comps : MonoBehaviour
    {
        public static GameObject LeftBtn;
        public static GameObject RightBtn;
        public static GameObject LoadBtn;

        public static Text AuthorText;
        public static Text NameText;
        public static Text DescriptionText;
        public static Text StatusText;
        //public static GameObject Credit;

        public static void SetupComps()
        {
            NameText = GameObject.Find("UI/Screen/Name").GetComponent<Text>();
            AuthorText = GameObject.Find("UI/Screen/Author").GetComponent<Text>();
            StatusText = GameObject.Find("UI/Screen/Current").GetComponent<Text>();
            DescriptionText = GameObject.Find("UI/Screen/Description").GetComponent<Text>();
            //Credit = GameObject.Find("UI/Screen/Credits");

            LoadBtn = GameObject.Find("Buttons/Load Btn");

            RightBtn = GameObject.Find("Buttons/Right Btn");
            LeftBtn = GameObject.Find("Buttons/Left Btn");
        }
    }
}
