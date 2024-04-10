"use client";
import { useEffect, useState } from "react";
import axios from "axios";
import Link from 'next/link';

export default function Home() {
  const [tasks, setTasks] = useState([]);

  useEffect(() => {
    async function loadTasks() {
      try {
        const response = await axios.get("http://localhost:5035/api/Tasks/AllTasks");
        setTasks(response.data);
      } catch (error) {
        console.error("Error loading tasks:", error);
      }
    }
    loadTasks();
  }, []);

  return (
    <div className="container">
      <main role="main" className="pb-3">
        <h1>Index</h1>
        <p>
          <Link href="/Create">Create New</Link>
        </p>
        <table className="table">
          <thead>
            <tr>
              <th>Description</th>
              <th>CreatedAtDt</th>
              <th>HasToBeDoneAtDt</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {tasks.map(function fn(task) {
              if (task.completedAtDt == null) {
                let time = Date.parse(task.hasToBeDoneAtDt);
                let hours = (time - new Date().getTime()) / (1000 * 60 * 60);
                let color = hours < 1 ? "text-danger" : "text-dark";
                return (
                  <tr key={task.id}>
                    <td className={color}>{task.description}</td>
                    <td className={color}>{task.createdAtDt}</td>
                    <td className={color}>{task.hasToBeDoneAtDt}</td>
                    <td>
                      <Link href={{
                        pathname: `/Edit/${task.id}`,
                        query: {id: task.id
                        }
                        ,
                        }}
                      >
                          Edit</Link> |{' '}
                      
                      <Link href={{pathname: `/Delete/${task.id}`,
                      query: {id: task.id
                      }
                      ,
                      }}>
                        Delete</Link>
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
