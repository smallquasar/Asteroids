using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameUIView : MonoBehaviour
    {
        [SerializeField] private TextMeshPro coordsText;
        [SerializeField] private TextMeshPro angleText;
        [SerializeField] private TextMeshPro velocityText;
        [SerializeField] private TextMeshPro laserCountText;
        [SerializeField] private TextMeshPro cooldownText;

        public void Update()
        {
            
        }
    }
}
