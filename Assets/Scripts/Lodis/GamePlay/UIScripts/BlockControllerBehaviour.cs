﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Lodis
{
    public class BlockControllerBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GameObjectList _globalBlockList;
        [SerializeField]
        private GameObjectList _blockListP1;

        public void AddBlockToP1(int index)
        {
            if (index <= 2)
            {
                _blockListP1.Objects[0] = _globalBlockList[index];
            }
            else if(index > 2 && index <= 5)
            {
                _blockListP1.Objects[1] = _globalBlockList[index];
            }
            else if(index > 5 && index <= 7)
            {
                _blockListP1.Objects[2] = _globalBlockList[index];
            }
            else
            {
                _blockListP1.Objects[3] = _globalBlockList[index];
            }
            
        }
    }
}


