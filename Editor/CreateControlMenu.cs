using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;

namespace Nightingale {
	public static class CreateControls {

		private static string UIStandard = "UI/Skin/UISprite.psd";
		private static string UIBackground = "UI/Skin/Background.psd";

		static Sprite GetStandardSprite() {
			return AssetDatabase.GetBuiltinExtraResource<Sprite>( UIStandard );
		}
		static Sprite GetBackgroundSprite() {
			return AssetDatabase.GetBuiltinExtraResource<Sprite>( UIBackground );
		}

		[MenuItem( "GameObject/UI Expansion/Tab Control", false, 10 )]
		static void CreateNewTabControl() {

			// タブグループを作成
			var new_obj = EditorUtility.CreateNewUIObject( "Tab Group", Selection.activeGameObject );
			var tab_script = new_obj.AddComponent<TabControlGroup>();
			var tab_rect_trans = new_obj.GetComponent<RectTransform>();

			tab_rect_trans.sizeDelta = new Vector2( tab_script.TabSize.x * 5, tab_script.TabSize.y );

			//DefaultControls.Resources res = new DefaultControls.Resources();
			tab_script.SelectedSprite = GetStandardSprite();
			tab_script.DeselectedSprite = GetBackgroundSprite();
			tab_script.DisableSprite = tab_script.DeselectedSprite;

			// セットでコンテンツルートになるオブジェクトを追加する。
			var new_content = EditorUtility.CreateNewUIObject( "Content", new_obj );
			var content_root = new_content.GetComponent<RectTransform>();
			content_root.sizeDelta = tab_rect_trans.sizeDelta;
			tab_script.ContentsRoot = content_root;

			// 簡単にひとつだけボタンを追加する
			TabControlGroupEditor.CreateNewTab( tab_script, content_root );

			tab_script.SelectedTabIndex = 0;
			EditorUtility.SetLayerRecursively( new_obj, new_obj.transform.parent.gameObject.layer );

			Undo.RegisterCreatedObjectUndo( new_obj, "Create Tab Control Group" );

			Selection.activeObject = new_obj;

		}

		[MenuItem( "GameObject/UI Expansion/Accordion Button", false, 10 )]
		static void CreateNewAccordionButton() {

			Vector2 size_delta = new Vector2( 120.0f, 30.0f );
			Vector2 content_size_delta = new Vector2( 120.0f, 120.0f );

			var new_obj = EditorUtility.CreateNewUIObject( "AccordionButton", Selection.activeGameObject, size_delta );
			var button_script = new_obj.AddComponent<AccordionButton>();
			button_script.CollapseSize = size_delta;
			button_script.ExpandSize = content_size_delta;

			// 隠れるコンテンツを設定する
			var content_mask = EditorUtility.CreateNewUIObject( "ContentMask", new_obj, content_size_delta );
			var content_mask_trans = content_mask.GetComponent<RectTransform>();
			content_mask.AddComponent<RectMask2D>();
			//content_mask_trans.localPosition = new Vector2( content_mask_trans.localPosition.x, -size_delta.y / 2.0f );
			content_mask_trans.pivot = new Vector2( 0.5f, 1.0f );
			content_mask_trans.localPosition = Vector2.zero;

			button_script.ContentsMask = content_mask_trans;

			var content = EditorUtility.CreateNewUIObject( "Content", content_mask, content_size_delta );
			var content_trans = content.GetComponent<RectTransform>();
			content_trans.anchorMin = new Vector2( 0.5f, 1.0f );
			content_trans.anchorMax = new Vector2( 0.5f, 1.0f );
			content_trans.pivot = new Vector2( 0.5f, 1.0f );
			content_trans.localPosition = Vector2.zero;

			button_script.ContentsRoot = content_trans;

			var background = EditorUtility.CreateNewUIObject( "Background", content, content_size_delta );
			var background_trans = background.GetComponent<RectTransform>();
			background_trans.anchorMin = new Vector2( 0.0f, 0.0f );
			background_trans.anchorMax = new Vector2( 1.0f, 1.0f );
			background_trans.offsetMin = Vector2.zero;
			background_trans.offsetMax = Vector2.zero;

			var background_image = background.AddComponent<Image>();
			background_image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>( UIStandard );
			background_image.type = Image.Type.Sliced;

			// ボタン部分を作成する
			var button = EditorUtility.CreateNewUIObject( "Header", new_obj, size_delta );
			var header_image = button.AddComponent<Image>();
			header_image.sprite = GetStandardSprite();
			header_image.type = Image.Type.Sliced;

			//// ボタンが閉じているイメージ
			//var collapse_image_obj = EditorUtility.CreateNewUIObject( "ImageCollapse", button, size_delta );
			//var collapse_image = collapse_image_obj.AddComponent<Image>();
			//collapse_image.sprite = GetStandardSprite();
			//collapse_image.type = Image.Type.Sliced;

			//// ボタンが開いたイメージ
			//var expand_image_obj = EditorUtility.CreateNewUIObject( "ImageExpand", button, size_delta );
			//var expand_image = expand_image_obj.AddComponent<Image>();
			//expand_image.sprite = GetStandardSprite();
			//expand_image.type = Image.Type.Sliced;

			// ボタンにのるテキスト
			var text_obj = EditorUtility.CreateNewUIObject( "Text", button, size_delta );
			var text = text_obj.AddComponent<Text>();
			text.text = "AccordionButton";
			text.color = new Color( 50f / 255f, 50f / 255f, 50f / 255f, 1f );
			text.alignment = TextAnchor.MiddleCenter;
			text.raycastTarget = false;

			button_script.CollapseSprite = GetStandardSprite();
			button_script.ExpandSprite = GetStandardSprite();
			button_script.HeaderImage = header_image;

			// 最後にこのコントロールを作った時の最初の状態を作る。
			button_script.isCollapsible = true;
			button_script.isExpand = true;

			EditorUtility.SetLayerRecursively( new_obj, new_obj.transform.parent.gameObject.layer );

			Undo.RegisterCreatedObjectUndo( new_obj, "Create Accordion Button" );

			Selection.activeObject = new_obj;

		}

	}

}