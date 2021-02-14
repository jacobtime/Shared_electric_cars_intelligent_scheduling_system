using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 共享电动车智能调度系统
{
    class Priority_queue
    {
        private Node[] array;
        private int size;
        public Priority_queue()
        {
            array = new Node[505];
        }
        public bool isEmpty()
        {
            return size == 0;
        }
        public void EnQueue(Node key)
        {
            if (size >= array.Length)
            {
                Resize();
            }
            array[size++] = key;
            UpAdjust();
        }
        public Node DeQueue()
        {
            if (size <= 0)
            {
                Console.WriteLine("the queue is empty!");
                return null;
            }
            Node head = array[0];
            array[0] = array[--size];
            DownAdjust();
            return head;
        }
        private void UpAdjust()
        {
            int childIndex = size - 1;
            int parentIndex = (childIndex - 1) / 2;
            Node temp = array[childIndex];
            while (childIndex > 0 && temp.CompareTo(array[parentIndex]) > 0)
            {
                array[childIndex] = array[parentIndex];
                childIndex = parentIndex;
                parentIndex = parentIndex / 2;
            }
            array[childIndex] = temp;
        }
        public void DownAdjust()
        {
            int parentIndex = 0;
            Node temp = array[parentIndex];
            int childIndex = 1;
            while (childIndex < size)
            {
                if (childIndex + 1 < size && array[childIndex + 1].CompareTo(array[childIndex]) > 0)
                {
                    childIndex++;
                }
                if (temp.CompareTo( array[childIndex]) >= 0)
                {
                    break;
                }
                array[parentIndex] = array[childIndex];
                parentIndex = childIndex;
                childIndex = 2 * childIndex + 1;
            }
            array[parentIndex] = temp;
        }
        
        private void Resize()
        {
            int newSize = this.size * 2;
            Node[] newArray = new Node[newSize];
            Array.Copy(this.array, 0, newArray, 0, this.size);
            array = newArray;
        }

    }
}
