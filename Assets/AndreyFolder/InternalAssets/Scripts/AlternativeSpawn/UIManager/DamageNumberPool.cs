using UnityEngine;
using TMPro;
using System.Collections;

public class DamageNumberPool : MonoCache
{
    private static DamageNumberPool instance;

    [SerializeField] public Transform _canvasTransform;
    [SerializeField] public TextMeshPro _textMeshPrefab;

    private PoolObject<TextMeshPro> _textPool;
    private TextMeshPro _textPrefab;
    private DamageNumber _damageNumber;

    public static DamageNumberPool Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<DamageNumberPool>();
            return instance;
        }
    }

    private void Start()
    {
        _textPrefab = _textMeshPrefab.GetComponent<TextMeshPro>();
        _damageNumber = _textMeshPrefab.GetComponent<DamageNumber>();

        PoolObject<TextMeshPro>.CreateInstance(_textPrefab, 20,
            _canvasTransform, "TextContainer");
        _textPool = PoolObject<TextMeshPro>.Instance;
    }

    public void Initialize(int damageAmount, Transform target, Color color)
    {
        TextMeshPro text = _textPool.GetObjects(target.position, _textMeshPrefab);

        text.SetText(damageAmount.ToString());
        text.color = color;
        _damageNumber._direction = (target.position - target.position).normalized;
        _damageNumber._direction += new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        _damageNumber._direction.Normalize();
        StartCoroutine(Duration(text));
    }

    private IEnumerator Duration(TextMeshPro text)
    {
        yield return new WaitForSeconds(_damageNumber._duration);
        _textPool.ReturnObject(text);
    }
}
