using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Composite.Core
{
    public class AbstractView : MonoBehaviour
    {
        public void Destroy() => Destroy(gameObject);

        public void SetActive(bool value) => gameObject.SetActive(value);
    }
}
