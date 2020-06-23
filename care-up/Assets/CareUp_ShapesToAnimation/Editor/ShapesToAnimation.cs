using UnityEngine;
using UnityEditor;

class ShapesToAnimation : EditorWindow
{

    public GameObject modelFile;
    public AnimationClip animationFile;

    int startFrame = 0;
    int endFrame = 100;
    bool loopAnim = false;
    float framesPerSecond = 60.0f;
    [MenuItem("Tools/Shapes To Animation")]
    static void Init()
    {
        EditorWindow.GetWindow<ShapesToAnimation>();
    }

    void OnGUI()
    {
        modelFile = EditorGUILayout.ObjectField(modelFile, typeof(GameObject), true) as GameObject; ;
        animationFile = EditorGUILayout.ObjectField(animationFile, typeof(AnimationClip), true) as AnimationClip;

        if (modelFile != null && animationFile != null)
        {
            if (GUILayout.Button("Run", GUILayout.Width(250)))
                CheckShapes(modelFile);
            loopAnim = EditorGUILayout.Toggle("Loop animation", loopAnim);
            startFrame = EditorGUILayout.IntField("Start frame: ", startFrame);
            endFrame = EditorGUILayout.IntField("End frame: ", endFrame);
            framesPerSecond = EditorGUILayout.FloatField("Animation FPS: ", framesPerSecond);
        }
    }

    void CheckShapes(GameObject AObject)
    {
        int blendShapeCount;
        SkinnedMeshRenderer skinnedMeshRenderer;
        Mesh skinnedMesh;
        skinnedMeshRenderer = AObject.GetComponent<SkinnedMeshRenderer>();
        skinnedMesh = AObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
        blendShapeCount = skinnedMesh.blendShapeCount;
        if (endFrame > blendShapeCount || endFrame <= 0)
            endFrame = blendShapeCount;
        AnimationClip clip = (AnimationClip)AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(animationFile), typeof(AnimationClip));

        if (loopAnim)
        {
            AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);
            settings.loopTime = true;
            AnimationUtility.SetAnimationClipSettings(clip, settings);
        }
        for (int i = 0; i < blendShapeCount; i++)
        {
            string blendName = skinnedMesh.GetBlendShapeName(i);
            AnimationCurve curve = null;
            clip.SetCurve("", typeof(SkinnedMeshRenderer), "blendShape." + blendName, curve);
        }

        for (int i = 0; i < blendShapeCount; i++)
        {
            string blendName = skinnedMesh.GetBlendShapeName(i);
            Keyframe[] keys = new Keyframe[3];
            if (i >= startFrame && i <= endFrame)
            {
                int frame = i - startFrame;
                if (i == startFrame && loopAnim)
                {
                    keys = new Keyframe[5];
                    keys[0] = new Keyframe((frame - 1) / framesPerSecond, 0.0f);
                    keys[1] = new Keyframe(frame / framesPerSecond, 100.0f);
                    keys[2] = new Keyframe((frame + 1) / framesPerSecond, 0.0f);
                    keys[3] = new Keyframe((endFrame - 1) / framesPerSecond, 0.0f);
                    keys[4] = new Keyframe(endFrame / framesPerSecond, 100.0f);
                }
                else
                {
                    keys[0] = new Keyframe((frame - 1) / framesPerSecond, 0.0f);
                    keys[1] = new Keyframe(frame / framesPerSecond, 100.0f);
                    keys[2] = new Keyframe((frame + 1) / framesPerSecond, 0.0f);
                }
            }
            else
            {
                keys = new Keyframe[1];
                keys[0] = new Keyframe(0.0f, 0.0f);
            }

            AnimationCurve curve = new AnimationCurve(keys);
            clip.SetCurve("", typeof(SkinnedMeshRenderer), "blendShape." + blendName, curve);
        }
        string assetPath = AssetDatabase.GetAssetPath(animationFile);
        //AssetDatabase.CreateAsset(clip, assetPath);
        // animationFile = AssetDatabase.LoadAssetAtPath(assetPath, typeof(AnimationClip)) as AnimationClip;
        AssetDatabase.SaveAssets();
    }
}