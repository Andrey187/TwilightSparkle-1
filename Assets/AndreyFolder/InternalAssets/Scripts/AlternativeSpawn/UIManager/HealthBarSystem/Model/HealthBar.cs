using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoCache
{
	[SerializeField] private Slider slider;
	[SerializeField] private Gradient gradient;
	[SerializeField] public Image Fill;
	[SerializeField] public Image Border;

    protected override void OnEnabled()
    {
		if (!gameObject.CompareTag("HealthBar"))
        {
			Fill.enabled = false;
			Border.enabled = false;
        }
	}

    public void SetMaxHealth(int health)
	{
		slider.maxValue = health;
		slider.value = health;

		Fill.color = gradient.Evaluate(1f);
	}

	public void SetHealth(int health)
	{
		slider.value = health;
		Fill.color = gradient.Evaluate(slider.normalizedValue);
	}
}
