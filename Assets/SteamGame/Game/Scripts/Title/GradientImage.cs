
using UnityEngine;
using UnityEngine.UI;

namespace TechC
{

    [RequireComponent(typeof(Image))]
    public class GradientImage : BaseMeshEffect
    {
        public enum GradientDirection
        {
            Vertical,
            Horizontal
        }

        [SerializeField] private Gradient gradient = new Gradient(); // Gradient型でインスペクター上に表示
        [SerializeField] private GradientDirection direction = GradientDirection.Vertical; // グラデーション方向

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive() || vh.currentVertCount == 0)
                return;

            UIVertex vertex = new UIVertex();
            int count = vh.currentVertCount;

            // Bounds計算
            Vector2 min = Vector2.positiveInfinity;
            Vector2 max = Vector2.negativeInfinity;

            for (int i = 0; i < count; i++)
            {
                vh.PopulateUIVertex(ref vertex, i);
                min = Vector2.Min(min, vertex.position);
                max = Vector2.Max(max, vertex.position);
            }

            float range = direction == GradientDirection.Vertical ? max.y - min.y : max.x - min.x;

            for (int i = 0; i < count; i++)
            {
                vh.PopulateUIVertex(ref vertex, i);

                // グラデーション方向に応じて位置を計算
                float position = direction == GradientDirection.Vertical
                    ? (vertex.position.y - min.y) / range // 縦方向
                    : (vertex.position.x - min.x) / range; // 横方向

                // Gradientから色を取得して適用
                vertex.color = gradient.Evaluate(position);
                vh.SetUIVertex(vertex, i);
            }
        }
    }


}
