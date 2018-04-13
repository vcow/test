using Properties;
using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(Card))]
    public class CardInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var card = target as ICard;
            if (card != null)
            {
                card.UpdateCard();
            }
        }
    }
}