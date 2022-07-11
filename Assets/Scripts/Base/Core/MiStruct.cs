using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BXB
{
    namespace Core
    {
        [System.Serializable]
        public struct Attribute
        {
            public ulong id;
            public float HP; 
            public float ATK;
            public float DEF;
            public Attribute(ulong id,float HP,float ATK,float DEF)
            {
                this.id = id;
                this.HP = HP;
                this.ATK = ATK;
                this.DEF = DEF;
            }
        }
        [System.Serializable]
        public struct Knapsack
        {
            public List<ulong> equip;
            public ulong glodCoin;
            public ulong diaMonds;
            public Knapsack(List<ulong> equip,ulong glodCoin,ulong diaMonds)
            {
                this.equip = equip;
                this.glodCoin = glodCoin;
                this.diaMonds = diaMonds;
            }
        }

        public struct Valuable
        {
            public ulong id;
            public ulong cros;
            public Valuable(ulong id, ulong cros)
            {
                this.id = id;
                this.cros = cros;
            }
        }

        public struct CharacterControllerInfo
        {
            public bool isGround;
            public bool isWall;
            public uint jumpMaxcCount;
            public uint nowJumpCount;
            public float bloodProportion;
            public float nowBlood;
            public float maxBlood;
        }
        [System.Serializable]
        public struct HandleControllerInfo
        {
            public float moveController;
            public float handleAngle;
            public float angleRadius;
        }
        public struct CommonGameObjectInfo
        {
            public ulong id;
            public string name;
            public float presentBlood;
            public float MaxBlood;
            public float proportionBlood;
            public float lastBlood;
            public CommonGameObjectInfo(object o)
            {
                id = 000000000;
                name = $"{id}";
                presentBlood = 0;
                MaxBlood = 0;
                proportionBlood = 0;
                lastBlood = 0;
            }
            public void SetBlood(float presentBlood)
            {
                lastBlood = this.presentBlood;
                this.presentBlood = presentBlood;
                proportionBlood = presentBlood / MaxBlood;
            }
        }
        //------ui
    }
}
