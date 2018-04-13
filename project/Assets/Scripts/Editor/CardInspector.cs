using Properties;
using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(Card))]
    public class CardInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            var p = serializedObject.GetIterator();
            while (p.NextVisible(true))
            {
                EditorGUILayout.PropertyField(p);
            }
            
            var card = (Card) target;
            card.Type = (CardType) EditorGUILayout.EnumPopup("Type", card.Type);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}