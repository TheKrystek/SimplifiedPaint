using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedPaint
{
    public class SlidingWindowList<T>
    {
        int position = -1;
        int maxSize;
        T[] array;


        public SlidingWindowList(int size = 5)
        {
            maxSize = size;
            array = new T[size];
        }

        public void Add(T newItem)
        {
            if (position + 1 >= maxSize)
                moveDown();
            else
                position++;

            array[position] = newItem;
        }

        public void MoveCursorDown()
        {
            if (position - 1 >= -1)
                position--;
        }

        public void MoveCursorUp()
        {
            if (position + 1 < maxSize)
                position++;
        }


        public T Get()
        {
            return array[position];
        }


        

        public T GetAt(int index)
        {
            return array[index];
        }

        private void moveDown(int step = 1)
        {
            if (step >= maxSize)
                return;

            for (int i = step; i < maxSize; i++)
                array[i - step] = array[i];
        }


        public void Display()
        {
            for (int i = 0; i < maxSize; i++)
            {
                Console.WriteLine("{2}{0}  {1}", i, array[i],(i == position) ? ">" : " ");
            }
        }

        public int Position
        {
            get
            {
                return position;
            }
        }

        public bool IsEmpty
        {
            get { return position == -1; }
        }

    }
}
