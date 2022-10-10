using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ubiq.XR;
using UnityEngine;
using Ubiq.Spawning;
using Ubiq.Messaging;

namespace Ubiq.Samples
{
    public interface IGraspableCube
    {
        void Attach(Hand hand);
    }

    /// <summary>
    /// The Fireworks Box is a basic interactive object. This object uses the NetworkSpawner
    /// to create shared objects (fireworks).
    /// The Box can be grasped and moved around, but note that the Box itself is *not* network
    /// enabled, and each player has their own copy.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class GraspableCube : MonoBehaviour, IGraspable, INetworkSpawnable
    {
        public GameObject FireworkPrefab;
        public NetworkSpawnManager nsm;
        private Hand follow;
        private Rigidbody body;

        public NetworkId NetworkId { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
        }

        public void Start()
        {
            nsm.SpawnWithPeerScope(gameObject);
        }

        public void Grasp(Hand controller)
        {
            follow = controller;
        }

        public void Release(Hand controller)
        {
            follow = null;
        }

        public void UnUse(Hand controller)
        {
        }


        private void Update()
        {
            if (follow != null)
            {
                transform.position = follow.transform.position;
                transform.rotation = follow.transform.rotation;
                body.isKinematic = true;
            }
            else
            {
                body.isKinematic = false;
            }
        }
    }
}