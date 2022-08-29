
using UnityEngine;
namespace Mao 
{
    public class Bomb : MonoBehaviour
    {
        public const float g = 9.8f * 2.0F;
        public GameObject target;  
        public float speed = 10;  
        private float verticalSpeed;  
        private Vector3 moveDirection;  
        
        private float angleSpeed;  
        private float angle;  
        private float time;  

        public delegate void xxFlowHandler(Vector3 pos,  FighterData td);

        protected xxFlowHandler PlayEnd;

        FighterData _td;

        void Start()  
        {}  

        public float excute(xxFlowHandler clickOK, FighterData td)
        {
            PlayEnd = clickOK;
            _td = td;

            float tmepDistance = Vector3.Distance(transform.position, target.transform.position);  
            float tempTime = tmepDistance / speed;  
            float riseTime, downTime;  
            riseTime = downTime = tempTime / 2;  
            verticalSpeed = g * riseTime;  
            transform.LookAt(target.transform.position);  

            float tempTan = verticalSpeed / speed;  
            double hu = Mathf.Atan(tempTan);  
            angle = (float)(180 / Mathf.PI * hu);   //  （将要转换的放在分子上） 或者直接使用 hu * Mathf.Rad2Deg
            transform.eulerAngles = new Vector3(-angle, transform.eulerAngles.y, transform.eulerAngles.z);  
            angleSpeed = angle / riseTime;  
            moveDirection = target.transform.position - transform.position;  

            return tempTime*2;

        }

        void Update()  
        {  
            if(target == null)
              return;

            if(transform.position.y < target.transform.position.y)  
            {  
                Vector3 pos = new Vector3(transform.position.x, 0.5F, transform.position.z);
                PlayEnd(pos, _td);
                //finish  
                InstanceManager.Instance.DeCreate(transform);
                return;  
            }
            time += Time.deltaTime;  
            float test = verticalSpeed - g * time;  
            transform.Translate(moveDirection.normalized * speed * Time.deltaTime, Space.World);  
            transform.Translate(Vector3.up * test * Time.deltaTime,Space.World);  
            float testAngle = -angle + angleSpeed * time;  
            transform.eulerAngles = new Vector3(testAngle, transform.eulerAngles.y, transform.eulerAngles.z);  
        }
    }
}
