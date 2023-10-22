using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsLaucnher : MonoBehaviour
{
    [SerializeField] private bool _startDisaster = false;


	// Start is called before the first frame update
	void Start()
    {
		

	}

    // Update is called once per frame
    void Update()
    {
        if (_startDisaster)
        {
            _startDisaster = false;

		}
    }

    public void OnLavaDisasterStarted()
    {
		
	}
}
