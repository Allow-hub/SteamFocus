
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

        [SerializeField] private Gradient gradient = new Gradient(); // Gradient�^�ŃC���X�y�N�^�[��ɕ\��
        [SerializeField] private GradientDirection direction = GradientDirection.Vertical; // �O���f�[�V��������

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive() || vh.currentVertCount == 0)
                return;

            UIVertex vertex = new UIVertex();
            int count = vh.currentVertCount;

            // Bounds�v�Z
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

                // �O���f�[�V���������ɉ����Ĉʒu���v�Z
                float position = direction == GradientDirection.Vertical
                    ? (vertex.position.y - min.y) / range // �c����
                    : (vertex.position.x - min.x) / range; // ������

                // Gradient����F���擾���ēK�p
                vertex.color = gradient.Evaluate(position);
                vh.SetUIVertex(vertex, i);
            }
        }
    }


}
