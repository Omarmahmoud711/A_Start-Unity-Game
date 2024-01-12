using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//so that a heap can be used for anything not just nodes so it's a generic of type T
public class Heap<T> where T : Heapo<T>
{

    T[] items;
    int current_item_count;

    //constructor

    public Heap(int max_heap_size)
    { // we already know that the size is width * length
        items = new T[max_heap_size]; //now the items is the array of size of the max heap size

    }

    // to add a new item to the heap
    // there is a problem that we are using a generic type T that could be anything not just an integer , so we need an interface to the main class to be implementing so
    //that we are using integers

    public void Add(T item)
    {   // after making the constructor now we can say things like = , < , > and so on .....
        // so now we are making the refrence to the index that we have is equal to itemcount
        //and we add the item to the end of the heap even though it's not it's right place so we need to sort it upwards
        item.HeapIndex = current_item_count;
        items[current_item_count] = item;
        sort_up(item);
        current_item_count++;//here we increased the itemcount by one since we added it to the heap
    }


    public void sort_up(T item) {
        int parent_index = (item.HeapIndex - 1) / 2;//now we have the parent index

        while (true) {
            T parent_item = items[parent_index];//this is the parent value
            if (item.CompareTo(parent_item) > 0)
            { //that means the parent is greater than the child so we are in min heap so we swap the parent with the child
                swap(item, parent_item);
            }
            else {
                break;

            }
            parent_index = (item.HeapIndex-1) / 2;
        }

    
    
    }

    // here we swapped the values and the indexes
    public void swap(T item, T parent_item) {
        items[item.HeapIndex] = parent_item;
        items[parent_item.HeapIndex] = item;
        int temp = item.HeapIndex;
        item.HeapIndex = parent_item.HeapIndex;
        parent_item.HeapIndex = temp;
    }


    public T get_lowest_Fcost() {
        T first_item = items[0];// we popped out the first item in the min heap which is the lowest
        current_item_count--;
        //now get the last element in the heap and put it up
        items[0] = items[current_item_count];
        items[0].HeapIndex = 0;
        //then sift down
        sort_down(items[0]);
        return first_item;
    }

    public void sort_down(T item) {
        int left_child_index = item.HeapIndex * 2 + 1;
        int right_child_index = item.HeapIndex * 2 + 2;
        int swap_index = 0;
        if (left_child_index < current_item_count)
        {
            swap_index = left_child_index;//default
            if (right_child_index < current_item_count)
            {
                if (items[left_child_index].CompareTo(items[right_child_index]) < 0)
                {
                    swap_index = right_child_index;
                }
            }
            if (item.CompareTo(items[swap_index]) < 0)
            {
                swap(item, items[swap_index]);
            }
            else
            {
                return;

            }

        }
        else { return;
        }
    }

    public bool contains(T item) {
        return Equals(items[item.HeapIndex], item);
    }

    public int count{
        get {
            return current_item_count;
        }
            }

    public void update(T item) {
        sort_up(item);
    }

}

//here is the interface that has the heap index constructor
    public interface Heapo<T> : IComparable<T> { 
    int HeapIndex {
            get;
            set;
        }
    }


