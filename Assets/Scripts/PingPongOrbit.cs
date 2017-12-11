using UnityEngine;

public class PingPongOrbit : MonoBehaviour
{
    public Vector3 RotationStart;
    public Vector3 RotationEnd;
    public float Speed;

    bool m_PositiveDirection = true;
    float m_CurrentValue = 0f;
	
	// Update is called once per frame
	void Update ()
    {
        float increment = Speed * Time.deltaTime;
        if (!m_PositiveDirection)
            increment *= -1f;
        m_CurrentValue += increment;

        float clampedValue = Mathf.Clamp(m_CurrentValue, 0f, 1f);
        if(m_CurrentValue != clampedValue)
        {
            m_PositiveDirection = !m_PositiveDirection;
        }

        transform.rotation = Quaternion.Slerp(Quaternion.Euler(RotationStart), Quaternion.Euler(RotationEnd), m_CurrentValue);
	}
}
