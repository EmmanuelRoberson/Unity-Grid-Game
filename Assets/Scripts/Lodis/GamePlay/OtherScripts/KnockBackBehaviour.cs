﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Lodis
{
    public class KnockBackBehaviour : MonoBehaviour
    {
        Rigidbody objectRigidbody;
        RaycastHit ray;
        GameObject hitTarget;
        RaycastHit[] raycastPanelHits;
        [SerializeField]
        private Event onKnockback;
        // Use this for initialization
        void Start()
        {
            objectRigidbody = GetComponent<Rigidbody>();
        }
        IEnumerator Stun(float stunTime)
        {
            GetComponent<InputButtonBehaviour>().enabled = false;
            BlackBoard.grid.onStun.Raise(gameObject);
            yield return new WaitForSeconds(stunTime);
            GetComponent<InputButtonBehaviour>().enabled = true;
            BlackBoard.grid.onStopStun.Raise(gameObject);
        }
        public void KnockBack(Vector3 direction,float power,float stunTime = 0)
        {
            onKnockback.Raise();
            if (CompareTag("Player"))
            {
                StartCoroutine(Stun(stunTime));
            }
            Vector3 rayDirection = new Vector3(0, 0, direction.z);
            raycastPanelHits = Physics.RaycastAll(transform.position, rayDirection, 3);
            int layerMask = 1 << 9;
           
            if(Physics.Raycast(transform.position, rayDirection, out ray, 3, layerMask))
            {
                objectRigidbody.AddForce(direction * power, ForceMode.Impulse);
                hitTarget = ray.transform.gameObject;
                return;
            }
            if (CompareTag("Player"))
            {
                GetComponent<GamePlay.OtherScripts.ScreenShakeBehaviour>().shouldStop = true;
                GameObject newPanel = raycastPanelHits[raycastPanelHits.Length - 1].transform.gameObject;
                PlayerMovementBehaviour playerMoveScript = GetComponent<PlayerMovementBehaviour>();
                if(newPanel.CompareTag("Panel"))
                {
                    playerMoveScript.CurrentPanel = newPanel;
                    playerMoveScript.ResetPositionToCurrentPanel();
                }
            }
            else if(CompareTag("Block"))
            {
                GetComponent<GamePlay.OtherScripts.ScreenShakeBehaviour>().shouldStop = true;
                GameObject newPanel = raycastPanelHits[raycastPanelHits.Length - 1].transform.gameObject;
                BlockBehaviour blockScript = GetComponent<BlockBehaviour>();
                if(newPanel.CompareTag("Panel"))
                {
                    blockScript.currentPanel = newPanel;
                    transform.position = new Vector3(newPanel.transform.position.x, transform.position.y, newPanel.transform.position.z);
                }
                
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject == hitTarget)
            {
                objectRigidbody.velocity = Vector3.zero;
                other.GetComponent<HealthBehaviour>().takeDamage(5);
                if (CompareTag("Player"))
                {
                    GetComponent<GamePlay.OtherScripts.ScreenShakeBehaviour>().shouldStop = true;
                    PlayerMovementBehaviour playerMoveScript = GetComponent<PlayerMovementBehaviour>();
                    playerMoveScript.ResetPositionToCurrentPanel();
                }
                else if (CompareTag("Block"))
                {
                    GetComponent<GamePlay.OtherScripts.ScreenShakeBehaviour>().shouldStop = true;
                    BlockBehaviour blockScript = GetComponent<BlockBehaviour>();
                    transform.position = new Vector3(blockScript.currentPanel.transform.position.x, transform.position.y, blockScript.currentPanel.transform.position.z);
                }
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject == hitTarget)
            {
                objectRigidbody.velocity = Vector3.zero;
                collision.gameObject.GetComponent<HealthBehaviour>().takeDamage(5);
                if (CompareTag("Player"))
                {
                    GetComponent<GamePlay.OtherScripts.ScreenShakeBehaviour>().shouldStop = true;
                    PlayerMovementBehaviour playerMoveScript = GetComponent<PlayerMovementBehaviour>();
                    playerMoveScript.ResetPositionToCurrentPanel();
                }
                else if (CompareTag("Block"))
                {
                    GetComponent<GamePlay.OtherScripts.ScreenShakeBehaviour>().shouldStop = true;
                    BlockBehaviour blockScript = GetComponent<BlockBehaviour>();
                    transform.position = new Vector3(blockScript.currentPanel.transform.position.x, transform.position.y, blockScript.currentPanel.transform.position.z);
                }
            }
        }
        // Update is called once per frame
        void Update()
        {
            Debug.DrawLine(transform.position, new Vector3(0,0,-3), Color.yellow);
        }
    }
}


