using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DayNightCycle : MonoBehaviour
{
    [Header("Ciclo")]
    [Tooltip("Minutos reales que dura un día completo (24h)")]
    public float dayLengthMinutes = 5f;
    [Tooltip("Hora de inicio en 24h, p.ej. 7 = 7AM")]
    [Range(0f, 24f)]
    public float startHour = 7f;

    [Header("Referencias")]
    public Light sunLight;
    public Light moonLight;

    [Header("Intensidades")]
    public float sunMaxIntensity  = 1f;
    public float sunMinIntensity  = 0f;
    public float moonMaxIntensity = 0.2f;
    public float moonMinIntensity = 0f;

    [Header("Skybox Procedural")]
    public Color daySkyTint    = new Color(0.5f, 0.5f, 0.6f);
    public Color nightSkyTint  = new Color(0.02f,0.05f,0.2f);
    public Color dayGroundColor   = new Color(0.3f,0.25f,0.2f);
    public Color nightGroundColor = new Color(0.01f,0.02f,0.05f);
    public float dayExposure   = 1f;
    public float nightExposure = 0.2f;

    private float secondsPerHour;
    private float currentHour;

    void Start()
    {
        // Calcula cuántos segundos dura una “hora” de juego
        dayLengthMinutes = Mathf.Max(0.1f, dayLengthMinutes);
        secondsPerHour   = (dayLengthMinutes*60f) / 24f;
        currentHour      = startHour % 24f;
    }

    void Update()
    {
        if (sunLight==null || moonLight==null) return;

        // 1) Avanza la hora
        currentHour += Time.deltaTime / secondsPerHour;
        if (currentHour >= 24f) currentHour -= 24f;

        // 3) Define un rawFactor que va de -∞ a +∞
    float rawFactor = Mathf.InverseLerp(6f, 18f, currentHour);

    // 4) Clamp a [0,1] para que fuera de 6–18h dé exactamente 0 (noche) o 1 (día)
    float dayFactor = Mathf.Clamp01(rawFactor);

    // 5) Rotación del padre tal cual ya lo tienes
    float angle = (currentHour / 24f) * 360f - 90f;
    transform.localRotation = Quaternion.Euler(new Vector3(angle, 0f, 0f));

    // 6) Intensidades del sol y la luna
    sunLight.intensity  = Mathf.Lerp(sunMinIntensity,  sunMaxIntensity,  dayFactor);
    moonLight.intensity = Mathf.Lerp(moonMaxIntensity, moonMinIntensity, 1f - dayFactor);

    // 7) Skybox procedural con factor correctamente clamppeado
    var sky = RenderSettings.skybox;
    if (sky.HasColor("_SkyTint"))
        sky.SetColor("_SkyTint", Color.Lerp(nightSkyTint,  daySkyTint,  dayFactor));
    if (sky.HasColor("_GroundColor"))
        sky.SetColor("_GroundColor", Color.Lerp(nightGroundColor, dayGroundColor, dayFactor));
    if (sky.HasFloat("_Exposure"))
        sky.SetFloat("_Exposure", Mathf.Lerp(nightExposure, dayExposure, dayFactor));
    }
}
