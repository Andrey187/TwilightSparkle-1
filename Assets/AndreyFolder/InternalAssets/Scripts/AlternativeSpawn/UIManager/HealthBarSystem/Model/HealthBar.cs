using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoCache
{
	[SerializeField] private Slider slider;
	[SerializeField] private Gradient gradient;
	[SerializeField] public Image Fill;
	[SerializeField] public Image Border;

    public void SetBaseMaxHealth(int health)
	{
		slider.maxValue = health;
		slider.value = health;

		Fill.color = gradient.Evaluate(1f);
	}

	public void SetDynamicMaxHealth(int maxHealth)
    {
		slider.maxValue = maxHealth;
	}

	public void SetHealth(int health)
	{
		slider.value = health;
		Fill.color = gradient.Evaluate(slider.normalizedValue);
	}
}
