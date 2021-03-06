﻿using System.Collections.Generic;
using UnityEngine;

namespace DragonBones
{
    public class UnityArmatureComponent : UnityEventDispatcher<EventObject>
    {
        /**
         * @private
         */
        public TextAsset dragonBonesJSON = null;

        /**
         * @private
         */
        public List<string> textureAtlasJSON = null;

        /**
         * @private
         */
        public string armatureName = null;

        /**
         * @private
         */
        public string animationName = null;

        /**
         * @private
         */
        internal Armature _armature = null;
        
        private bool _disposeGameObject = true;

        /**
         * @private
         */
        void Awake()
        {
            LoadData(dragonBonesJSON, textureAtlasJSON);

            if (DragonBones.IsAvailableString(armatureName))
            {
                UnityFactory.factory.BuildArmatureComponent(armatureName, null, null, this.gameObject);
            }

            if (_armature != null)
            {
                sortingLayerName = sortingLayerName;
                sortingOrder = sortingOrder;

                if (DragonBones.IsAvailableString(animationName))
                {
                    _armature.animation.Play(animationName);
                }
            }
        }

        /**
         * @private
         */
        void OnDestroy()
        {
        }

        /**
         * @private
         */
        override public void _onClear()
        {
            _armature = null;

            if (_disposeGameObject)
            {
#if UNITY_EDITOR
                Object.DestroyImmediate(this.gameObject);
#else
                Object.Destroy(this.gameObject);
#endif
            }

            _disposeGameObject = true;
        }

        /**
         * @private
         */
        public DragonBonesData LoadData(TextAsset dragonBonesJSON, List<string> textureAtlasJSON)
        {
            DragonBonesData dragonBonesData = null;

            if (dragonBonesJSON != null)
            {
                dragonBonesData = UnityFactory.factory.LoadDragonBonesData(dragonBonesJSON);

                if (dragonBonesData != null && textureAtlasJSON != null)
                {
                    foreach (var eachJSON in textureAtlasJSON)
                    {
                        UnityFactory.factory.LoadTextureAtlasData(eachJSON);
                    }
                }
            }

            return dragonBonesData;
        }

        public void Dispose(bool disposeGameObject)
        {
            _disposeGameObject = disposeGameObject;

            if (_armature != null)
            {
                _armature.Dispose();
                _armature = null;
            }
        }

        /**
         * @language zh_CN
         * 获取骨架。
         * @readOnly
         * @see dragonBones.Armature
         * @version DragonBones 4.5
         */
        public Armature armature
        {
            get { return _armature; }
        }

        /**
         * @language zh_CN
         * 获取动画控制器。
         * @readOnly
         * @see dragonBones.Animation
         * @version DragonBones 4.5
         */
        new public Animation animation
        {
            get { return _armature != null ? _armature.animation : null; }
        }

        [SerializeField]
        protected string _sortingLayerName = "Default";
        public string sortingLayerName
        {
            get { return _sortingLayerName; }
            set
            {
                if (_sortingLayerName == value)
                {
                    //return;
                }

                _sortingLayerName = value;

                foreach (var render in GetComponentsInChildren<Renderer>(true))
                {
                    render.sortingLayerName = value;
                }
            }
        }

        [SerializeField]
        protected int _sortingOrder = 0;
        public int sortingOrder
        {
            get { return _sortingOrder; }
            set
            {
                if (_sortingOrder == value)
                {
                    //return;
                }

                _sortingOrder = value;

                foreach (var render in GetComponentsInChildren<Renderer>(true))
                {
                    render.sortingOrder = value;
                }
            }
        }

        [SerializeField]
        protected float _zSpace = 0.0f;
        public float zSpace
        {
            get { return _zSpace; }
            set
            {
                if (value < 0.0f || float.IsNaN(value))
                {
                    value = 0.0f;
                }

                if (_zSpace == value)
                {
                    return;
                }

                _zSpace = value;

                foreach (var slot in _armature.GetSlots())
                {
                    var display = slot.display as GameObject;
                    if (display != null)
                    {
                        display.transform.localPosition = new Vector3(display.transform.localPosition.x, display.transform.localPosition.y, -slot._zOrder * (_zSpace + 0.001f));
                    }
                }
            }
        }
    }
}