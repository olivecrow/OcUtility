#if ENABLE_TEXT_MESH_PRO
using System;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace OcUtility.Editor
{
    public class TMPSpriteSearchWindow : EditorWindow
    {
        TMP_SpriteAsset _spriteAsset;
        TMP_SpriteCharacter _selected;
        Sprite _sprite;
        SerializedObject so;

        Texture2D _clipboardIcon;
        Vector2 _scrollPos;
        string _result;
        [UnityEditor.MenuItem("Utility/TMP 스프라이트 문자열 구하기")]
        public static void Open()
        {
            var window = GetWindow<TMPSpriteSearchWindow>(true);
            window.Show();
            window.maxSize = new Vector2(512, 720);
        }

        void OnEnable()
        {
            so = new SerializedObject(this);
            _clipboardIcon = Resources.Load<Texture2D>("clipboard icon");
        }

        void OnGUI()
        {
            GUILayout.Box("이미지 출력을 위한 리치텍스트를 적용해도 이미지가 보이지 않는 경우, \n" +
                          "에셋을 Assets/TextMesh Pro/Resources/Sprite Assets 폴더에 넣어야함.");
            so.Update();
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false;
            EditorGUILayout.TextField("Result", _result);
            GUI.enabled = true;
            if(GUILayout.Button(_clipboardIcon, GUILayout.Width(32))) 
                if(!string.IsNullOrWhiteSpace(_result)) GUIUtility.systemCopyBuffer = _result;
            EditorGUILayout.EndHorizontal();
            _spriteAsset = EditorGUILayout.ObjectField(_spriteAsset, typeof(TMP_SpriteAsset), false) as TMP_SpriteAsset;
            if (_spriteAsset != null)
            {
                var texture = _spriteAsset.spriteSheet as Texture2D;
                
                _scrollPos = EditorGUILayout.BeginScrollView(
                    _scrollPos, false, false, 
                    GUILayout.MaxWidth(position.width));
                var width = texture.width;
                var height = texture.height;
                foreach (var character in _spriteAsset.spriteCharacterTable)
                {
                    GUILayout.Label(character.name);
                    var backgroundColor = GUI.backgroundColor;
                    if(_selected == character)
                    {
                        GUI.backgroundColor = Color.cyan;
                    }
                    var glyph = character.glyph;
                    var sR = new Rect(glyph.glyphRect.x, glyph.glyphRect.y, glyph.glyphRect.width, glyph.glyphRect.height);

                    var ratio = 64 / sR.height;
                    var rect = EditorGUILayout.GetControlRect(false, 64, GUILayout.Width(sR.width * ratio));
                    var skin = new GUIStyle(GUI.skin.button);
                    skin.normal.background = GUI.skin.box.normal.background;
                    skin.fixedWidth = sR.width * ratio;
                    skin.fixedHeight = 64;
                    var selected = GUI.Button(rect, "", skin);

                    var normalizedPos = new Vector2(sR.x / width, sR.y / height);
                    var normalizedSize = new Vector2(sR.width / width, sR.height / height);
                    var normalizedCoord = new Rect(normalizedPos, normalizedSize);
                    GUI.DrawTextureWithTexCoords(rect, texture, normalizedCoord);
                    
                    EditorGUILayout.Space();
                    
                    GUI.backgroundColor = backgroundColor;
                    if (selected)
                    {
                        _selected = character;
                        _result = $"<sprite=\"{_spriteAsset.name}\" name=\"{character.name}\">";
                    }
                }
                EditorGUILayout.EndScrollView();
            }

            so?.ApplyModifiedProperties();
        }
        
        
    }
}
#endif