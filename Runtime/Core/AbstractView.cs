using UnityEngine;

namespace Composite.Core
{
    public class AbstractView : MonoBehaviour
    {
        public void Destroy() => Destroy(gameObject);

        public void SetActive(bool value) => gameObject.SetActive(value);
    }
}
