using System;
using System.Collections;
using System.Collections.Generic;
using Ubiq.Messaging;
using Ubiq.NetworkedBehaviour;
using UnityEngine;

namespace Ubiq.Samples
{
    /// <summary>
    /// This class shows how to implement a networked object using 
    /// NetworkedBehaviours.
    /// 
    /// In Start, the Component is registered with NetworkedBehaviours.Register
    /// and from then on all its serialisable members are synchronised
    /// automatically across all Peers.
    /// Serialisable members are public fields, and private fields with the
    /// [SerializeField] attribute.
    /// 
    /// This class also has the NetworkTransform attribute applied. When this
    /// attribute is present, the GameObjects Transform will be synchronised
    /// as well.
    /// </summary>
    [NetworkTransform]
    public class NetworkedBehaviourCube : MonoBehaviour
    {
        // The synchronisation follows the same rules as the Unity Json
        // serialiser: we can synchronise fields of simple types, including
        // our own types. We can also synchronise lists, but not dictionaries.

        private NetworkContext context;

        private Vector3 lastPosition;

        [Serializable]
        public class NestedClass
        {
            public int InternalInt;
        }

        public float Float;
        public string String;
        public NestedClass Class;
        public Color Color;
        private NetworkContext networkContext;

        private Material material;

        private void Awake()
        {
            material = GetComponent<MeshRenderer>().material;
        }

        void Start()
        {
            context = NetworkScene.Register(this);
            Debug.Log(this.networkContext.Id);
        }

        // Update is called once per frame
        void Update()
        {
            if (transform.localPosition != lastPosition)
            {
                material.color = Color;
                lastPosition = transform.localPosition;
                context.SendJson<Vector3>(lastPosition);
            }
        }

        public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
        {
            var newPosition = message.FromJson<Vector3>();
            lastPosition = newPosition;
            transform.localPosition = lastPosition;
        }
    }
}