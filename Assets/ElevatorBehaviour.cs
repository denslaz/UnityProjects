using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;
using System.Threading.Tasks;
public class ElevatorBehaviour : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    //Объявляем необходимые типы данных для последующей работы с ними
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    public int ElevatorPosition = 1;
    public string Floor1 = "Null", Floor2 = "Null", Floor3 = "Null", Floor4 = "Null", ElevatorCondition = "InActive", ElevatorDirection = "--";
    bool quit, stop = false;
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    //Здесь инициализируем кнопки, положения лифта, а также тестовые панели для вывода номера этажа и направления лифта
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    public Button Up1, Up2, Up3, Down2, Down3, Down4, FloorButton1, FloorButton2, FloorButton3, FloorButton4, Stop, Pause;
    public Image State1, State2, State3, State4;
    public Text ElevatorPanel, ElevatorDirect, Floor1Text, Floor2Text, Floor3Text, Floor4Text;


    //----------------------------------------------------------------------------------------------------------------------------------------------------
    //Функция Update обновляется каждый фрейм(frame), поэтому она отвечает за отображение изменения положения лифта и его направления на текстовых панелях
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    void Update()
    {
        ElevatorPanel.text = ElevatorPosition.ToString();
        ElevatorDirect.text = ElevatorDirection.ToString();
        Floor1Text.text = ElevatorPosition.ToString();
        Floor2Text.text = ElevatorPosition.ToString();
        Floor3Text.text = ElevatorPosition.ToString();
        Floor4Text.text = ElevatorPosition.ToString();
    }
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    /*IEnumerator - базовый интерфейс для всех перечислителей, здесь выполняется задержка выполнения кода в целях эмуляции реального оборудования
     Сюда передается число, отображающее на сколько секунд необходимо выполнить задержку */
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    IEnumerator wait(float waitTime)
    {
        float counter = 0;

        while (counter < waitTime)
        {
            counter += Time.deltaTime;
            if (quit)
            {
                yield break;
            }
            yield return null;
        }
    }

    //----------------------------------------------------------------------------------------------------------------------------------------------------
    //Данная функция отвечает за подъем лифта на определенный этаж, номер которого передается в переменную x
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    IEnumerator GoUp(int x)
    {
        int a = ElevatorPosition;
        if (a == x) //Если лифт уже находится на этаже с вызовом, то произойдет открытие дверей и посадка пассажиров
        {
            if (a == 1) { Floor1 = "Null"; }
            else if (a == 2) { Floor2 = "Null"; }
            else if (a == 3) { Floor3 = "Null"; }
            else if (a == 4) { Floor4 = "Null"; }
            yield return wait(3);
        }
        for (int i = ElevatorPosition; i != x; i++) //Цикл выполняется до тех пор, пока лифт не приедет на нужный этаж
        {
            ElevatorDirection = "Up";
            switch (ElevatorPosition)
            {
                case 1:
                    yield return wait(1);
                    //gameObject.setActive отвечает за активацию и деактивацию картинки лифта в приложении
                    State1.gameObject.SetActive(false);
                    State2.gameObject.SetActive(true);
                    Floor1 = "Null";
                    ElevatorPosition = 2;
                    break;
                case 2:
                    yield return wait(1);
                    if (Floor2 == "Up") //По пути, если направление лифта совпадает с кнопкой вызова, то лифт остановится
                    {
                        yield return wait(3);
                        Floor2 = "Null";//После ожидания статус вызова кнопки на данном этаже обнуляется 
                    }
                    State2.gameObject.SetActive(false);
                    State3.gameObject.SetActive(true);
                    ElevatorPosition = 3;
                    break;
                case 3:
                    yield return wait(1);
                    if (Floor3 == "Up")
                    {
                        yield return wait(3);
                        Floor3 = "Null";
                    }
                    State3.gameObject.SetActive(false);
                    State4.gameObject.SetActive(true);
                    ElevatorPosition = 4;
                    yield return wait(3);
                    break;
            }
            if (stop == true) //Данное условие срабатывает, когда нажимается кнопка STOP
            {
                stop = false;
                break;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------
        //Далее выполняются проверки на то, нажаты ли кнопки вызова на этажах или нет
        //Если нажаты кнопки 1 или 4 этажа, то сначала лифт поедет на них, поскольку по пути он может остановиться и подобрать людей на других этажах 
        //----------------------------------------------------------------------------------------------------------------------------------------------------
        if (Floor1 == "Up" && ElevatorPosition >= 1)
        {
            StartCoroutine(GoDown(1));
        }
        else if (Floor4 == "Down" && ElevatorPosition <= 4)
        {
            StartCoroutine(GoUp(4));
        }
        else if (Floor3 != "Null" && ElevatorPosition <= 3)
        {
            StartCoroutine(GoUp(3));
        }
        else if (Floor3 != "Null" && ElevatorPosition >= 3)
        {
            StartCoroutine(GoDown(3));
        }
        else if (Floor2 != "Null" && ElevatorPosition <= 2)
        {
            StartCoroutine(GoUp(2));
        }
        else if (Floor2 != "Null" && ElevatorPosition >= 2)
        {
            StartCoroutine(GoDown(2));
        }
        //Если вызовов больше нет, то лифт становится неактивным
        else { ElevatorDirection = "--"; ElevatorCondition = "InActive"; }
    }
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    //Данная функция отвечает за спуск лифта на определенный этаж и во многом похожа на функцию подъема
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    IEnumerator GoDown(int x)
    {
        int a = ElevatorPosition;
        if (a == x)
        {
            if (a == 1) { Floor1 = "Null"; }
            else if (a == 2) { Floor2 = "Null"; }
            else if (a == 3) { Floor3 = "Null"; }
            else if (a == 4) { Floor4 = "Null"; }
            yield return wait(3);//Ждем секунду для открытия дверей, секунду для посадки пассажиров и ещё секунду для закрытия дверей
        }
        for (int i = ElevatorPosition; i != x; i--)
        {
            ElevatorDirection = "Down";
            switch (ElevatorPosition)
            {
                case 2:
                    yield return wait(1); //Ждем секунду для симуляции ожидания перехода лифта с одного этажа на другой
                    if (Floor2 == "Down")
                    {
                        yield return wait(3);
                        Floor2 = "Null";
                    }
                    State2.gameObject.SetActive(false);
                    State1.gameObject.SetActive(true);
                    ElevatorPosition = 1;
                    yield return wait(3);
                    break;
                case 3:
                    yield return wait(1);
                    if (Floor3 == "Down")
                    {
                        yield return wait(3);
                        Floor3 = "Null";
                    }
                    State3.gameObject.SetActive(false);
                    State2.gameObject.SetActive(true);
                    ElevatorPosition = 2;
                    break;
                case 4:
                    yield return wait(1);
                    State4.gameObject.SetActive(false);
                    State3.gameObject.SetActive(true);
                    Floor4 = "Null";
                    ElevatorPosition = 3;
                    break;
            }
            if (stop == true)
            {
                stop = false;
                break;
            }
        }
        if (Floor1 == "Up" && ElevatorPosition > 1)
        {
            StartCoroutine(GoDown(1));
        }
        else if (Floor4 == "Down" && ElevatorPosition <= 4)
        {
            StartCoroutine(GoUp(4));
        }
        else if (Floor3 != "Null" && ElevatorPosition <= 3)
        {
            StartCoroutine(GoUp(3));
        }
        else if (Floor3 != "Null" && ElevatorPosition >= 3)
        {
            StartCoroutine(GoDown(3));
        }
        else if (Floor2 != "Null" && ElevatorPosition <= 2)
        {
            StartCoroutine(GoUp(2));
        }
        else if (Floor2 != "Null" && ElevatorPosition >= 2)
        {
            StartCoroutine(GoDown(2));
        }
        else { ElevatorDirection = "--"; ElevatorCondition = "InActive"; } 
    }

    //----------------------------------------------------------------------------------------------------------------------------------------------------
    //Далее идут функции, привязанные к определенным кнопкам и работающие в зависимости от того активен ли лифт или нет
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    public void FourthFloorButton()
    {
        if (ElevatorCondition == "InActive") //Если лифт неактивен, то вызываем его на этаж
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoUp(4));
        }
        else if (ElevatorCondition == "Active") //Если лифт активен, то выставляем статус кнопки в активный
        {
            Floor4 = "Down";
        }
    }
    public void ThirdFloorButtonUp()
    {
        if (ElevatorCondition == "InActive" && ElevatorPosition > 3)
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoDown(3));
        }
        else if (ElevatorCondition == "InActive" && ElevatorPosition < 3)
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoUp(3));
        }
        else if (ElevatorCondition == "Active")
        {
            Floor3 = "Up";
        }
    }
    public void ThirdFloorButtonDown()
    {
        if (ElevatorCondition == "InActive" && ElevatorPosition > 3)
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoDown(3));
        }
        else if (ElevatorCondition == "InActive" && ElevatorPosition < 3)
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoUp(3));
        }
        else if (ElevatorCondition == "Active")
        {
            Floor3 = "Down";
        }
    }
    public void SecondFloorButtonUp()
    {
        if (ElevatorCondition == "InActive" && ElevatorPosition > 2)
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoDown(2));
        }
        else if (ElevatorCondition == "InActive" && ElevatorPosition < 2)
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoUp(2));
        }
        else if (ElevatorCondition == "Active")
        {
            Floor2 = "Up";
        }
    }
    public void SecondFloorButtonDown()
    {
        if (ElevatorCondition == "InActive" && ElevatorPosition > 2)
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoDown(2));
        }
        else if (ElevatorCondition == "InActive" && ElevatorPosition < 2)
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoUp(2));
        }
        else if(ElevatorCondition == "Active")
        {
            Floor2 = "Down";
        }

    }
    public void FirstFloorButton()
    {
        if (ElevatorCondition == "InActive")
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoDown(1));
        }
        else if (ElevatorCondition == "Active")
        {
            Floor1 = "Up";
        }
    }
    public void Elevator4Floor()
    {
        if (ElevatorCondition == "InActive")
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoUp(4));
        }
        else if (ElevatorCondition == "Active")
        {
            Floor4 = "Down";
        }
    }
    public void Elevator3Floor()
    {
        if (ElevatorCondition == "InActive" && ElevatorPosition > 3)
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoDown(3));
        }
        else if (ElevatorCondition == "InActive" && ElevatorPosition < 3)
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoUp(3));
        }
        else if (ElevatorCondition == "Active" && ElevatorPosition <= 3)
        {
            Floor3 = "Up";
        }
        else if (ElevatorCondition == "Active" && ElevatorPosition >= 3)
        {
            Floor3 = "Down";
        }

    }
    public void Elevator2Floor()
    {
        if (ElevatorCondition == "InActive" && ElevatorPosition > 2)
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoDown(2));
        }
        else if (ElevatorCondition == "InActive" && ElevatorPosition < 2)
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoUp(2));
        }
        else if (ElevatorCondition == "Active" && ElevatorPosition <= 2)
        {
            Floor2 = "Up";
        }
        else if (ElevatorCondition == "Active" && ElevatorPosition >= 2)
        {
            Floor2 = "Down";
        }
    }
    public void Elevator1Floor()
    {
        if (ElevatorCondition == "InActive")
        {
            ElevatorCondition = "Active";
            StartCoroutine(GoDown(1));
        }
        else if (ElevatorCondition == "Active")
        {
            Floor1 = "Up";
        }
    }
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    //Кнопка STOP останавливает лифт на ближайшем этаже (согласно движению) и отменяет все вызовы лифта
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    public void STOP()
    {
        stop = true;
       Floor1 = "Null"; Floor2 = "Null"; Floor3 = "Null"; Floor4 = "Null"; ElevatorCondition = "InActive";
    }
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    //Кнопка PAUSE имитирует воспрепятстование закрытию дверей лифта, а так же экстренную остановку лифта на этаже
    //После чего продолжает выполнение вызовов
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    public void PAUSE()
    {
        Thread.Sleep(2000);
    }
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    //Данная кнопка в UNITY позволяет выйти из приложения
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    public void Exit()
    {
        Application.Quit();
    }

}
