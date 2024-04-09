'use client'
import { useEffect } from 'react';
import axios from 'axios';
import {IFormInterface} from "@/components/IFormInterface";

export const useTaskLoader = (id: string, setTaskData: IFormInterface) => {
    useEffect(() => {
        const loadTask = async () => {
            try {
                const response = await axios.get(`http://localhost:5035/api/Tasks/GetTask/${id}`);
                setTaskData.description = response.data.description;
                setTaskData.createdAtDt = response.data.createdAtDt;
                setTaskData.hasToBeDoneAtDt = response.data.hasToBeDoneAtDt;
                setTaskData.completedAtDt = response.data.hasToBeDoneAtDt;;
            } catch (error) {
                console.log('Error:', error);
            }
        };
        if (id) {
            loadTask();
        }
    }, [id, setTaskData]);
};