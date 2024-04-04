using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicineBoxController : MonoBehaviour
{
    GameObject antibody;
    [SerializeField] float cureSliderValue = 0.2f;
    private void Start()
    {
        //antibody = GameObject.FindGameObjectWithTag("Antibody"); // bu kodu begenmedim kendi childi olan objeyi daha rahat sekilde bulabilmem lazim /// cozuldu
        antibody = transform.Find("AntiBody").gameObject;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerController = other.GetComponent<PlayerController>();
            AntibodyMovement();
        }
    }
    public void AntibodyMovement()
    {
      
        EndGameSectionController.Instance.cureProgressModel.CureSliderIncrease(cureSliderValue);

        antibody.transform.DOJump(new Vector3(antibody.transform.position.x + UnityEngine.Random.Range(-1.5f, 1.5f), antibody.transform.position.y, antibody.transform.position.z + UnityEngine.Random.Range(-1.5f, 1.5f)), 2, 1, 1).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
