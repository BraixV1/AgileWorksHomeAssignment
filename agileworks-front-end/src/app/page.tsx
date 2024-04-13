"use client";
import {useEffect, useState} from "react";
import {useForm} from 'react-hook-form'
import axios from "axios";
import Link from 'next/link';
import {IFormInterface} from "@/components/IFormInterface";
import LoadingScreen from "@/components/LoadingScreen";

export default function Home() {
    const [tasks, setTasks] = useState<IFormInterface[]>([]);
    const {register, handleSubmit} = useForm<IFormInterface>();
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        async function loadTasks() {
            try {
                const response = await axios.get("http://localhost:5035/api/Tasks/AllTasks");
                setTasks(response.data);
                setLoading(false);
            } catch (error) {
                console.error("Error loading tasks:", error);
                setLoading(false);
            }
        }

        loadTasks();
    }, []);

    const onSubmit = async (data: IFormInterface) => {
        try {
            await axios.patch('http://localhost:5035/api/Tasks/CompleteTask/' + data.id, {
                id: data.id
            });
            location.reload();
        } catch (error) {
            console.log('Error: ', error)
        }
    }

    if (loading) {
        return <LoadingScreen/>;
    }

    return (
        <div className="container">
            <main role="main" className="pb-3">
                <h1>Tasks</h1>
                <p>
                    <Link href="/Create">Create New</Link>
                </p>
                <table className="table">
                    <thead>
                    <tr>
                        <th>Description</th>
                        <th>Created At</th>
                        <th>Has to be done</th>
                    </tr>
                    </thead>
                    <tbody>
                    {tasks.map(function fn(task) {
                        if (task.completedAtDt == null) {
                            let time = task.hasToBeDoneAtDt;
                            console.log('Time is' + time);
                            let hours = (new Date(time).getTime() - new Date().getTime()) / (1000 * 60 * 60);
                            let color = hours < 1 ? "text-danger" : "text-dark";
                            return (
                                <tr key={task.id}>
                                    <td className={color}>{task.description}</td>
                                    <td className={color}>{task.createdAtDt instanceof Date ? task.createdAtDt.toISOString() : task.createdAtDt}</td>
                                    <td className={color}>{task.hasToBeDoneAtDt instanceof Date ? task.hasToBeDoneAtDt.toISOString() : task.hasToBeDoneAtDt}</td>
                                    <td>
                                        <Link href={{
                                            pathname: `/Edit/${task.id}`,
                                            query: {
                                                id: task.id
                                            }
                                            ,
                                        }}
                                        >
                                            Edit</Link> |{' '}

                                        <Link href={{
                                            pathname: `/Delete/${task.id}`,
                                            query: {
                                                id: task.id
                                            }
                                            ,
                                        }}>
                                            Delete</Link>
                                        <form onSubmit={handleSubmit(onSubmit)}>
                                            <input {...register('id')} value={task.id} type="hidden"/>
                                            <input type="submit" value='Complete Task' className="btn btn-outline-success btn-sm"/>
                                        </form>
                                    </td>
                                </tr>
                            );
                        }
                    })}
                    </tbody>
                </table>
            </main>
        </div>
    );
}
