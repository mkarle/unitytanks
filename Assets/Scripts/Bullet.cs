using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    [SerializeField] private GameObject explosion;
    [SerializeField]
    private int explosionRadius = 40;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    void OnDestroy()
    {
        Vector2 explosionPos = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, (float)explosionRadius / 100);

        for (int i = 0; i < colliders.Length; i++)
        {
            // TODO: two calls for getcomponent is bad
            if (colliders[i].GetComponent<DestructibleSprite>())
                colliders[i].GetComponent<DestructibleSprite>().ApplyDamage(explosionPos, explosionRadius);
        }
    }
}
