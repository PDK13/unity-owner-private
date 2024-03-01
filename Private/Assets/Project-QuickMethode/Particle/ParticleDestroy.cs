public class ParticleDestroy : ParticlePlay
{
    public override void SetStart()
    {
        base.SetStart();
        Destroy(m_base.gameObject, m_base.main.duration + m_base.main.startLifetimeMultiplier);
    }
}