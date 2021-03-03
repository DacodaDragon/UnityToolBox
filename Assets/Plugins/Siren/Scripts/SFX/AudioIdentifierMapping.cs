using UnityEngine;
using System;

namespace Siren
{
    /// <summary>
    /// DataClass: Maps an indentifier to an audio asset
    /// </summary>
    [Serializable]
    public class AudioIdentifierMapping
    {
        [SerializeField] private string _Identifier = "id";
		[SerializeField] private AudioAsset _AudioAsset = null;
        
        public string Identifier => _Identifier;
        public AudioAsset AudioAsset => _AudioAsset;

    }
}
