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
        private Hand follow;
        private Rigidbody body;

        public NetworkId NetworkId { get; set; }

        private NetworkContext context;
        private void Start()
        {
            context = NetworkScene.Register(this);
        }
        private void Awake()
        {
            body = GetComponent<Rigidbody>();
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

        public struct Message
        {
            public TransformMessage transform;

            public Message(Transform transform)
            {
                this.transform = new TransformMessage(transform);
            }
        }

        private void Update()
        {
            if (follow != null)
            {
                transform.position = follow.transform.position;
                transform.rotation = follow.transform.rotation;
                context.SendJson(new Message(transform));
                body.isKinematic = true;
            }
            else
            {
                body.isKinematic = false;
            }
        }
    }
}