using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Universe_Controller : MonoBehaviour
{
    //las 2 cámaras del scenario
    public Camera Camara_Principal, Camara_B;
    //Los layer que las cámaras pueden visualizar
    public LayerMask universo_A, universo_B;


    //bool para controlar a que universo cambia
    public bool cambiar;
    //bool para controlar que si presiona muchas veces
    public bool press;
    //los perfiles que se van a modificar
    public UnityEngine.Rendering.VolumeProfile volumeProfile;
    UnityEngine.Rendering.Universal.Vignette vignette;
    UnityEngine.Rendering.Universal.ColorAdjustments Escala_gris;

    //las curvas de animación para la cámara
    public AnimationCurve _fov;
    public AnimationCurve _timeScale;
    public AnimationCurve Vingette;
    public AnimationCurve Saturacion;

    public AudioSource _audio;
    public GameObject[] espejo;

    private void Awake()
    {
        // se carga la escena paralela 
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }
    void Start()
    {
        //obtenemos todos los objetos de array
        espejo = GameObject.FindGameObjectsWithTag("espejo");
        _audio = GetComponent<AudioSource>();
    }
  
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z)&&press)
        {//Al presionar z se inicia la corrutina 
            StartCoroutine(SwapAsync());
            press = false;
        }
    }


    IEnumerator SwapAsync()
    {
        _audio.PlayOneShot(_audio.clip);
        //se le dice a la cámara principal que tome las curvas de animación y los efectos....
        #region
        for (float t = 0; t < 1.0f; t += Time.unscaledDeltaTime * 1.2f)
        {
            Camara_Principal.fieldOfView = _fov.Evaluate(t);
            Time.timeScale = _timeScale.Evaluate(t);


            if (!volumeProfile.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));
            vignette.intensity.Override(Vingette.Evaluate(t));


            if (!volumeProfile.TryGet(out Escala_gris)) throw new System.NullReferenceException(nameof(Escala_gris));
            Escala_gris.saturation.Override(Saturacion.Evaluate(t));
            yield return null;
        }
        #endregion


        //decide que universo seleccionar
        #region 
        cambiar = !cambiar;
        cambiar_Universo();
        #endregion

        //fieldOfView de la cámara principal regresa a su estado inicial y los efectos de post pro
        Camara_Principal.fieldOfView = _fov.Evaluate(1.0f);
        vignette.intensity.Override(Vingette.Evaluate(0.4f));
        Escala_gris.saturation.Override(Vingette.Evaluate(0.1f));
       // Time.timeScale = 1.0f;
        press = true;
    }
    public void cambiar_Universo()
    {
        if (!cambiar)
        {
            Camara_Principal.cullingMask = universo_A;
            Camara_B.cullingMask = universo_B;
            for (int x = 0; x < espejo.Length; x++)
            {
                espejo[x].layer = 12;
            }
        }
        else
        {
            Camara_Principal.cullingMask = universo_B;
            Camara_B.cullingMask = universo_A;
            for (int x = 0; x < espejo.Length; x++)
            {
                espejo[x].layer = 13;
            }
        }
        
    }

}
