﻿using System;
using UnityEngine;

namespace Lodis.GamePlay.BlockScripts
{
	public class DefenseBarrierBehaviour : MonoBehaviour,IUpgradable
	{
		[SerializeField] private HealthBehaviour healthScript;
		private float _decayVal;
		[SerializeField] private int _upgradeVal;
		private string colorName;
		private Material _attachedMaterial;
        [SerializeField]
        private BlockBehaviour _blockScript;
		[SerializeField] private RoutineBehaviour shieldTimer;
        private string _nameOfItem;
        private float lerpNum;
		// Use this for initialization
		void Start ()
		{
			_decayVal = (float)1/shieldTimer.actionLimit;
			lerpNum = (float)1/shieldTimer.actionLimit;
			colorName = "Color_262603E3";
			_attachedMaterial = GetComponent<MeshRenderer>().material;
            _nameOfItem = gameObject.name;
		}
        public BlockBehaviour block
        {
            get
            {
                return _blockScript;
            }
            set
            {
                _blockScript = value;
            }
        }
        public GameObject specialFeature
        {
            get
            {
                return gameObject;
            }
        }

        public string Name
        {
            get
            {
                return _nameOfItem;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            ResolveCollision(other.gameObject);
        }

        public void UpgradeBlock(GameObject otherBlock)
		{
			BlockBehaviour _blockScript = otherBlock.GetComponent<BlockBehaviour>();
            foreach (IUpgradable component in _blockScript.componentList)
            {
                if (component.Name == Name)
                {
                    component.specialFeature.GetComponent<DefenseBarrierBehaviour>().Upgrade();
					return;
				}
			}
			TransferOwner(otherBlock);
		}
        /// <summary>
        /// Upgrades:
        /// Increases health
        /// Replenishes shield
        /// </summary>
		public void Upgrade()
		{
			gameObject.SetActive(true);
			healthScript.health.Val += _upgradeVal;
			shieldTimer.actionLimit += 10;
			shieldTimer.ResetActions();
			_decayVal = (float)1/shieldTimer.actionLimit;
			lerpNum = (float)1/shieldTimer.actionLimit;
			_attachedMaterial.SetColor(colorName,Color.Lerp(Color.green, Color.red,lerpNum));
		}
		public void DestroyBarrier()
		{
			healthScript.health.Val = 5;
			gameObject.SetActive(false);
		}
        //Changes the color of the sphere to reflect how much time it has left
		public void DecaySphere()
		{
			_attachedMaterial.SetColor(colorName,Color.Lerp(Color.green, Color.red,lerpNum));
			lerpNum += _decayVal;
		}
        /// <summary>
        /// Transfers shield to other block
        /// while also healing other block.
        /// </summary>
        /// <param name="otherBlock"></param>
		public void TransferOwner(GameObject otherBlock)
		{
			BlockBehaviour blockScript = otherBlock.GetComponent<BlockBehaviour>();
			healthScript = otherBlock.GetComponent<HealthBehaviour>();
			healthScript.health.Val += _upgradeVal;
			blockScript.componentList.Add(this);
			transform.SetParent(otherBlock.transform,false);
		}
        //Tells the block to take damage
        public void ResolveCollision(GameObject collision)
        {
            if (collision.CompareTag("Projectile"))
            {
                block.HealthScript.takeDamage(collision.GetComponent<BulletBehaviour>().DamageVal);
                collision.GetComponent<BulletBehaviour>().Destroy();
            }
        }
        //Stops shield from decaying
        public void ActivateDisplayMode()
        {
            shieldTimer.StopAllCoroutines();
            shieldTimer.enabled = false;
        }
    }
}
