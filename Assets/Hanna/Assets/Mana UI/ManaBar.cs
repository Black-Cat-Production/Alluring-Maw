using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour{
	
	private Mana mana;
	private float BarMaskWidth;
	private RectTransform BarMaskRectTransform;
	private RawImage BarRawImage;
	
    private void Awake(){
		BarMaskRectTransform = transform.Find("BarMask").GetComponent<RectTransform>();
		BarRawImage = transform.Find("BarMask").Find("Bar").GetComponent<RawImage>();
		
		BarMaskWidth = BarMaskRectTransform.sizeDelta.x;
		
		mana = new Mana();
	}
	
	private void Update(){
		mana.Update();
		
		//BarImage.fillAmount = mana.GetManaNormalized();
		Rect uvRect = BarRawImage.uvRect;
		uvRect.x += .15f * Time.deltaTime; //panning speed
		BarRawImage.uvRect = uvRect;
		
		Vector2 BarMaskSizeDelta = BarMaskRectTransform.sizeDelta;
		BarMaskSizeDelta.x = mana.GetManaNormalized() * BarMaskWidth;
		BarMaskRectTransform.sizeDelta = BarMaskSizeDelta;
		
		if (Input.GetKeyDown(KeyCode.M)){
			int manaToSpend = 20;
			mana.TrySpendMana(manaToSpend);
			Debug.Log("Mana spent: " + manaToSpend);
			
		}
	}
}


public class Mana {
	
	public const int MANA_MAX = 100;
	
	private float manaAmount;
	private float manaRegenAmount;
	
	public Mana() {
		manaAmount = 0;
		manaRegenAmount = 20f;
	}
	
	public void Update() { //Regen rate
		manaAmount += manaRegenAmount * Time.deltaTime;
		manaAmount = Mathf.Clamp(manaAmount, 0f, MANA_MAX);
	}
	
	public void TrySpendMana(int amount) {
		if (manaAmount >= amount) {
			manaAmount -= amount;
		}
	}
	
	public float GetManaNormalized() {
		return manaAmount / MANA_MAX;
	}
}